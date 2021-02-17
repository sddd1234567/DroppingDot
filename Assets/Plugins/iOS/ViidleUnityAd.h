//
//  ViidleUnityAd.h
//  Unity-iPhone
//

#import <ViidleSDK/ViidleSDK.h>

typedef const void *ViidleAdIOSPtr;
typedef const void *ViidleAdUnityPtr;
typedef void (*ViidleAdUnityLoadCompletedCallback)(ViidleAdUnityPtr unityPtr);
typedef void (*ViidleAdUnityLoadFailedCallback)(ViidleAdUnityPtr unityPtr, NSInteger errorCode, const char* errorMessage);
typedef void (*ViidleAdUnityShowVideoFailedCallback)(ViidleAdUnityPtr unityPtr);
typedef void (*ViidleAdUnityVideoDisplayedCallback)(ViidleAdUnityPtr unityPtr);
typedef void (*ViidleAdUnityRewardCompletedCallback)(ViidleAdUnityPtr unityPtr, const char* currencyName, NSInteger currencyAmount);
typedef void (*ViidleAdUnityVideoClosedCallback)(ViidleAdUnityPtr unityPtr);

extern "C" {
    ViidleAdIOSPtr _CreateViidleAd(ViidleAdUnityPtr unityPtr, const char* unitId, ViidleAdUnityLoadCompletedCallback loadCompletedCallback, ViidleAdUnityLoadFailedCallback loadFailedCallback, ViidleAdUnityShowVideoFailedCallback showVideoFailedCallback, ViidleAdUnityVideoDisplayedCallback videoDisplayedCallback, ViidleAdUnityRewardCompletedCallback rewardCompletedCallback, ViidleAdUnityVideoClosedCallback videoClosedCallback);
    void _LoadViidleAd(ViidleAdIOSPtr iOSPtr);
    void _ShowViidleAd(ViidleAdIOSPtr iOSPtr);
    void _TestMode(ViidleAdIOSPtr iOSPtr);
    void _SetAutoReload(ViidleAdIOSPtr iOSPtr, bool isAutoReload);
    bool _IsReady(ViidleAdIOSPtr iOSPtr);
    void _SetUserId(ViidleAdIOSPtr iOSPtr, const char* userId);
    void _ReleaseViidleAd(ViidleAdIOSPtr iOSPtr);
}
