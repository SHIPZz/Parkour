﻿using CodeBase.Enums;
using CodeBase.Services.Ad;
using CodeBase.Services.SaveSystems.Data;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class LevelLoadState : IState, IPayloadedEnter<WorldData>
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IAdInvokerService _adInvokerService;

        public LevelLoadState(ILoadingCurtain loadingCurtain, IAdInvokerService adInvokerService)
        {
            _adInvokerService = adInvokerService;
            _loadingCurtain = loadingCurtain;
        }

        public async void Enter(WorldData payload)
        {
            _loadingCurtain.FillHalf(1.5f);
            
            payload.LevelData.Id = Mathf.Clamp(payload.LevelData.Id, 1, SceneManager.sceneCountInBuildSettings - 1);

            if (payload.LevelData.Id > 1) 
                _adInvokerService.Invoke();


            AsyncOperation loadSceneAsync;

#if UNITY_EDITOR
            loadSceneAsync =  SceneManager.LoadSceneAsync(EditorPrefs.GetString("Level id"));
            
            #else
             loadSceneAsync = SceneManager.LoadSceneAsync(payload.LevelData.Id);
#endif
            
            while (!loadSceneAsync.isDone || _adInvokerService.AdEnabled)
                await UniTask.Yield();
            
            _loadingCurtain.Show(1.5f);
        }
    }
}