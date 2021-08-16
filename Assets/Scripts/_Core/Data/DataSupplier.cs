using UnityEngine;
using System.Collections;


namespace GameFramework.Core
{
    [System.Serializable]
    public class DataSupplier<T>
    {
        #region Variables

        [SerializeField] DataId id;

        #endregion



        #region Public methods

        public T GetData()
        {
            return DataVault.Instance.Pull<T>(id);
        }


        public void SetData(T data)
        {
            DataVault.Instance.Push(data, id);
        }

        #endregion

    }
}