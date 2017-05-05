﻿using System;
using System.Collections.Generic;

namespace Orchard.ContentManagement
{
    public class DefaultContentManagerSession : IContentManagerSession
    {
        private readonly Dictionary<int, ContentItem> _itemByVersionId = new Dictionary<int, ContentItem>();
        private readonly Dictionary<Tuple<string, int>, ContentItem> _itemByContentItemId = new Dictionary<Tuple<string, int>, ContentItem>();
        private readonly Dictionary<string, ContentItem> _publishedItemsById = new Dictionary<string, ContentItem>();
        private readonly IDictionary<string, ContentItem> _latestItemsById = new Dictionary<string, ContentItem>();

        private bool _hasItems;

        public void Store(ContentItem item)
        {
            _hasItems = true;

            _itemByVersionId.Add(item.Id, item);
            _itemByContentItemId.Add(Tuple.Create(item.ContentItemId, item.Number), item);

            // is it the  Published version ?
            if (item.Published)
            {
                _publishedItemsById[item.ContentItemId] = item;
            }

            // is it the latest version ?
            if (item.Latest)
            {
                _latestItemsById[item.ContentItemId] = item;
            }
        }

        public bool RecallVersionId(int id, out ContentItem item)
        {
            if (!_hasItems)
            {
                item = null;
                return false;
            }

            return _itemByVersionId.TryGetValue(id, out item);
        }

        public bool RecallContentItemId(string contentItemId, int versionNumber, out ContentItem item)
        {
            if (!_hasItems)
            {
                item = null;
                return false;
            }

            if (_itemByContentItemId.TryGetValue(Tuple.Create(contentItemId, versionNumber), out item))
            {
                if (item.Number != versionNumber)
                {
                    _itemByContentItemId.Remove(Tuple.Create(contentItemId, versionNumber));
                    item = null;
                }
            }

            return item != null;
        }

        public bool RecallPublishedItemId(string id, out ContentItem item)
        {
            if (!_hasItems)
            {
                item = null;
                return false;
            }

            if (_publishedItemsById.TryGetValue(id, out item))
            {
                if (!item.Published)
                {
                    _publishedItemsById.Remove(id);
                    item = null;
                }
            }

            return item != null;
        }

        public bool RecallLatestItemId(string id, out ContentItem item)
        {
            if (!_hasItems)
            {
                item = null;
                return false;
            }

            if (_latestItemsById.TryGetValue(id, out item))
            {
                if (!item.Latest)
                {
                    _latestItemsById.Remove(id);
                    item = null;
                }
            }

            return item != null;
        }

        public void Clear()
        {
            _itemByVersionId.Clear();
            _itemByContentItemId.Clear();
            _publishedItemsById.Clear();
            _latestItemsById.Clear();
            _hasItems = false;
        }
    }
}