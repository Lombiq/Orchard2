using System;

namespace OrchardCore.ContentManagement.GraphQL.Options
{
    public class GraphQLContentPartOption<TContentPart> : GraphQLContentPartOption where TContentPart : ContentPart
    {
        public GraphQLContentPartOption() : base(nameof(TContentPart))
        {
        }
    }

    public class GraphQLContentPartOption
    {
        public GraphQLContentPartOption(string contentPartName)
        {
            if (string.IsNullOrEmpty(contentPartName))
            {
                throw new ArgumentNullException(nameof(contentPartName));
            }

            Name = contentPartName;
        }

        public string Name { get; }

        public bool Collapse { get; set; }
        public bool Ignore { get; set; }
    }
}