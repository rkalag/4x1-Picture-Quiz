function UnityProgress(unityInstance, progress) {
  if (!unityInstance.Module)
    return;
  if (!unityInstance.logo) {
    unityInstance.logo = document.createElement("div");
    unityInstance.logo.className = "logo " + unityInstance.Module.splashScreenStyle;
    unityInstance.container.appendChild(unityInstance.logo);
  }
  if (!unityInstance.progress) {    
    unityInstance.progress = document.createElement("div");
    unityInstance.progress.className = "progress " + unityInstance.Module.splashScreenStyle;
    unityInstance.progress.empty = document.createElement("div");
    unityInstance.progress.empty.className = "empty";
    unityInstance.progress.appendChild(unityInstance.progress.empty);
    unityInstance.progress.full = document.createElement("div");
    unityInstance.progress.full.className = "full";
    unityInstance.progress.appendChild(unityInstance.progress.full);
    unityInstance.container.appendChild(unityInstance.progress);
  }
  unityInstance.progress.full.style.width = (100 * progress) + "%";
  unityInstance.progress.empty.style.width = (100 * (1 - progress)) + "%";
  if(isInstantGame)
  {
    FBInstant.setLoadingProgress(progress * 100);
  }
  console.log("__________111", progress);
  if (progress == 1)
  {
    console.log("__________111", isInstantGame);
    if(isInstantGame)
			{
        console.log("__________111");
        GetMobileOS();
        console.log("__________111");
		    MobileOrDesktop();
        
        unityInstance.SendMessage('PlayerData', 'GetPlayerData', JSON.stringify(fbPlayerData));
        console.log("__________111");
				FBInstant.startGameAsync().then(function () {
					supportedAPIs = FBInstant.getSupportedAPIs();
          // if (supportedAPIs.includes('loadBannerAdAsync')) {
          //   setTimeout(() => {
          //     LoadBanner();
          //   }, 2000);
						
					// }
					// else
					// {
					// 	console.log("@@@@@@@@@@@@@@______NO BANNER ADS");
					// }
					if (supportedAPIs.includes('getInterstitialAdAsync')) {
						loadInterstitial();
					}
					else
					{
						//console.log("@@@@@@@@@@@@@@______NO INTERSTITIAL ADS");
					}
					if (supportedAPIs.includes('getRewardedVideoAsync')) {
						loadRewardVideo();
					}
					else
					{
						//console.log("@@@@@@@@@@@@@@______NO REWARDS ADS");
					}
          FBInstant.onPause(function () {
						//console.log("App paused");
					});
					facebookStuff.name = FBInstant.player.getName();
          facebookStuff.picture = FBInstant.player.getPhoto();
          unityInstance.logo.style.display = unityInstance.progress.style.display = "none";
          CheckInApp();
          unityInstance.SendMessage('PlayerData', 'GetMyName', facebookStuff.name);
					toBase64(imgURL, function(dataUrl) {
					  base64Picture = dataUrl;
					});

					toBase64(facebookStuff.picture, function(dataUrl) {
					  base64Picture2 = dataUrl;
					});
					checkShortcutCreation();
				});
			}
			else{
        //console.log("_______________no instant game");
        //unityInstance.SendMessage('Data', 'GetPlayerData', JSON.stringify(fbPlayerData));
        unityInstance.logo.style.display = unityInstance.progress.style.display = "none";
			}    
  }
}