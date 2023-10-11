// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using System.Collections;

// public class MemorySave : MonoBehaviour
// {
//     [SerializeField] private AssetReference mesh;
//     [SerializeField] private AssetReference material;
//     private AsyncOperationHandle _currentSkyboxMaterialOperationHandle;


//     void OnEnable()
//     {
//         StartCoroutine(SetMaterial());
//     }

//     private IEnumerator SetMaterial()
//     {
//         if (_currentSkyboxMaterialOperationHandle.IsValid())
//         {
//             Addressables.Release(_currentSkyboxMaterialOperationHandle);
//         }

//         AssetReference materialReference = material;
//         _currentSkyboxMaterialOperationHandle = materialReference.LoadAssetAsync<Material>();
//         yield return _currentSkyboxMaterialOperationHandle;
//         this.GetComponent<MeshRenderer>().material = _currentSkyboxMaterialOperationHandle.Result;
//     }


//     void OnDisable()
//     {

//     }
// }
