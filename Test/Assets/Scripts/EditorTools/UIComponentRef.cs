using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class UIComponentRef : MonoBehaviour
    {
        public ItemInfo root;
        public List<ItemInfo> itemInfoList;

        [Serializable]
        public class ItemInfo
        {
            public GameObject gameObject;
            public string fieldName;
            public string typeName;
        }
    }
}
