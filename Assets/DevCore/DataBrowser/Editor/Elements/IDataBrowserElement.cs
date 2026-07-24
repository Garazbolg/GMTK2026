using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevCore.DataBrowser.Editor
{
    public interface IDataBrowserElement {
        public string serializationKey { get; }
        
        public string GetSerializedData();
        public void ApplySerializedData(string textData);
    }

    [Serializable]
    public class SerializableData<T> {
        public T value;

        public SerializableData(T value) {
            this.value = value;
        }
    }
}
