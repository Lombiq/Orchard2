using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Apis.GraphQL.Queries;
using OrchardCore.ContentManagement.GraphQL.Queries.Types;
using OrchardCore.ContentManagement.Records;
using YesSql;

namespace OrchardCore.ContentManagement.GraphQL.Queries
{
    /// <summary>
    /// This type is used by <see cref="ContentItemFieldTypeProvider"/> to represent a query on a content type
    /// </summary>
    public class ContentItemsFieldType : FieldType
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContentItemsFieldType(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            Name = "ContentItems";

            Type = typeof(ListGraphType<ContentItemType>);

            Arguments = new QueryArguments(
                new QueryArgument<PublicationStatusGraphType> { Name = "status", Description = "publication status of the content item", DefaultValue = PublicationStatusEnum.Published },
                new QueryArgument<StringGraphType> { Name = "contentType", Description = "type of content item" },
                new QueryArgument<StringGraphType> { Name = "contentItemId", Description = "content item id" },
                new QueryArgument<StringGraphType> { Name = "contentItemVersionId", Description = "the id of the version" },
                new QueryArgument<ContentItemOrderByInput> { Name = "orderBy", Description = "order the response" }
            );

            Resolver = new AsyncFieldResolver<IEnumerable<ContentItem>>(Resolve);
        }

        private async Task<IEnumerable<ContentItem>> Resolve(ResolveFieldContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var contentManager = httpContext.RequestServices.GetService<IContentManager>();

            PublicationStatusEnum status = PublicationStatusEnum.Published;

            var versionOption = VersionOptions.Published;

            if (context.HasPopulatedArgument("status"))
            {
                status = context.GetArgument<PublicationStatusEnum>("status");
            }

            switch (status)
            {
                case PublicationStatusEnum.Published: versionOption = VersionOptions.Published; break;
                case PublicationStatusEnum.Draft: versionOption = VersionOptions.Draft; break;
                case PublicationStatusEnum.Latest: versionOption = VersionOptions.Latest; break;
            }

            if (context.HasPopulatedArgument("contentItemId"))
            {
                var contentItemId = context.GetArgument<string>("contentItemId");

                var contentItem = await contentManager.GetAsync(contentItemId, versionOption);

                if (contentItem == null)
                {
                    return Enumerable.Empty<ContentItem>();
                }

                return new[] { contentItem };
            }

            if (context.HasPopulatedArgument("contentItemVersionId"))
            {
                var contentItem = await contentManager.GetVersionAsync(context.GetArgument<string>("contentItemVersionId"));

                if (contentItem == null)
                {
                    return Enumerable.Empty<ContentItem>();
                }

                return new[] { contentItem };
            }

            var session = httpContext.RequestServices.GetService<YesSql.ISession>();
            var queryFilters = httpContext.RequestServices.GetServices<IGraphQLFilter<ContentItem>>();

            var query = session.Query<ContentItem, ContentItemIndex>();

            if (versionOption.IsPublished)
            {
                query = query.Where(q => q.Published == true);
            }
            else if (versionOption.IsDraft)
            {
                query = query.Where(q => q.Latest == true && q.Published == false);
            }
            else if (versionOption.IsLatest)
            {
                query = query.Where(q => q.Latest == true);
            }

            if (context.HasPopulatedArgument("contentType"))
            {
                var value = context.GetArgument<string>("contentType");
                query = query.Where(q => q.ContentType == value);
            }
            else
            {
                var value = (context.ReturnType as ListGraphType).ResolvedType.Name;
                query = query.Where(q => q.ContentType == value);
            }

            IQuery<ContentItem> contentItemsQuery = query;

            foreach (var filter in queryFilters)
            {
                contentItemsQuery = filter.PreQuery(contentItemsQuery, context);
            }

            var contentItems = await contentItemsQuery.ListAsync();

            foreach (var filter in queryFilters)
            {
                contentItems = filter.PostQuery(contentItems, context);
            }

            return contentItems.ToList();
        }
    }

    public class ContentItemOrderByInput : InputObjectGraphType
    {
        public ContentItemOrderByInput()
        {
            Name = "ContentItemOrderBy";

            Field<OrderByDirectionGraphType>("contentItemId");
            Field<OrderByDirectionGraphType>("contentItemVersionId");
            Field<OrderByDirectionGraphType>("contentType");
            Field<OrderByDirectionGraphType>("displayText");
            Field<OrderByDirectionGraphType>("published");
            Field<OrderByDirectionGraphType>("latest");
            Field<OrderByDirectionGraphType>("modifiedUtc");
            Field<OrderByDirectionGraphType>("publishedUtc");
            Field<OrderByDirectionGraphType>("createdUtc");
            Field<OrderByDirectionGraphType>("owner");
            Field<OrderByDirectionGraphType>("author");
        }
    }

    public enum OrderByDirection
    {
        Ascending,
        Descending
    }

    public class OrderByDirectionGraphType : EnumerationGraphType
    {
        public OrderByDirectionGraphType()
        {
            Name = "OrderByDirection";
            Description = "the order by direction";
            AddValue("ASC", "orders content items in ascending order", OrderByDirection.Ascending);
            AddValue("DESC", "orders content items in descending order", OrderByDirection.Descending);
        }
    }
}
