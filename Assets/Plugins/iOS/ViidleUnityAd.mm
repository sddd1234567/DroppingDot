//
//  ViidleUnityAd.m
//  Unity-iPhone
//

#import "ViidleUnityAd.h"
#import "ViidleUnityGlobal.h"

extern UIViewController* UnityGetGLViewController();

typedef NS_ENUM(NSInteger, ViidleAdCallback) {
    ViidleLoadCompleted = 0,
    viidleLoadFailed,
    viidleShowFailed,
    viidleVideoDisplayed,
    viidleVideoCompleted,
    viidleRewardCompleted
};

//==============================================================================

@interface ViidleAd : NSObject <ViidleDelegate>

@property (nonatomic) Viidle *viidle;
@property (nonatomic) ViidleAdUnityPtr unityPtr;
@property (nonatomic) ViidleAdUnityLoadCompletedCallback loadCompletedCallback;
@property (nonatomic) ViidleAdUnityLoadFailedCallback loadFailedCallback;
@property (nonatomic) ViidleAdUnityShowVideoFailedCallback showVideoFailedCallback;
@property (nonatomic) ViidleAdUnityVideoDisplayedCallback videoDisplayedCallback;
@property (nonatomic) ViidleAdUnityRewardCompletedCallback rewardCompletedCallback;
@property (nonatomic) ViidleAdUnityVideoClosedCallback videoClosedCallback;

@end

//==============================================================================

@implementation ViidleAd

- (void)viidleLoadCompleted {
    if (self.loadCompletedCallback) {
        self.loadCompletedCallback(self.unityPtr);
    }
}

- (void)viidleLoadFailedWithErrorCode:(NSInteger)errorCode errorMessage:(NSString *)errorMessage {
    if (self.loadFailedCallback) {
        self.loadFailedCallback(self.unityPtr, errorCode, (char*)[errorMessage UTF8String]);
    }
}

- (void)viidleShowVideoFailed {
    if (self.showVideoFailedCallback) {
        self.showVideoFailedCallback(self.unityPtr);
    }
}

- (void)viidleVideoDisplayed {
    if (self.videoDisplayedCallback) {
        self.videoDisplayedCallback(self.unityPtr);
    }
}

- (void)viidleRewardCompletedWithCurrencyName:(NSString *)currencyName currencyAmount:(NSInteger)currencyAmount {
    if (self.rewardCompletedCallback) {
        self.rewardCompletedCallback(self.unityPtr, (char*)[currencyName UTF8String], currencyAmount);
    }
}

- (void)viidleVideoCompleted {
    if (self.videoClosedCallback) {
        self.videoClosedCallback(self.unityPtr);
    }
}

@end

///-----------------------------------------------
/// @name C Interfaces
///-----------------------------------------------

ViidleAdIOSPtr _CreateViidleAd(ViidleAdUnityPtr unityPtr, const char* unitId, ViidleAdUnityLoadCompletedCallback loadCompletedCallback, ViidleAdUnityLoadFailedCallback loadFailedCallback, ViidleAdUnityShowVideoFailedCallback showVideoFailedCallback, ViidleAdUnityVideoDisplayedCallback videoDisplayedCallback, ViidleAdUnityRewardCompletedCallback rewardCompletedCallback, ViidleAdUnityVideoClosedCallback videoClosedCallback)
{
    ViidleAd *viidleAd = [[ViidleAd alloc] init];
    viidleAd.viidle = [[Viidle alloc] initWithUnitId:ViidleUnityCreateNSString(unitId)];
    viidleAd.viidle.delegate = viidleAd;
    viidleAd.unityPtr = unityPtr;
    viidleAd.loadCompletedCallback = loadCompletedCallback;
    viidleAd.loadFailedCallback = loadFailedCallback;
    viidleAd.showVideoFailedCallback = showVideoFailedCallback;
    viidleAd.videoDisplayedCallback = videoDisplayedCallback;
    viidleAd.rewardCompletedCallback = rewardCompletedCallback;
    viidleAd.videoClosedCallback = videoClosedCallback;
    ViidleUnityCacheObject(viidleAd);
    return (__bridge ViidleAdIOSPtr)viidleAd;
}

void _LoadViidleAd(ViidleAdIOSPtr iOSPtr)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    [viidleAd.viidle load];
}

void _ShowViidleAd(ViidleAdIOSPtr iOSPtr)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    dispatch_async(dispatch_get_main_queue(), ^{
        [viidleAd.viidle showWithViewController:UnityGetGLViewController()];
    });
}

void _TestMode(ViidleAdIOSPtr iOSPtr) {
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    dispatch_async(dispatch_get_main_queue(), ^{
        [viidleAd.viidle presentTestModeWithViewController:UnityGetGLViewController()];
    });
}

void _SetAutoReload(ViidleAdIOSPtr iOSPtr, bool isAutoReload)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    [viidleAd.viidle setAutoReload: isAutoReload];
}

bool _IsReady(ViidleAdIOSPtr iOSPtr)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    return viidleAd.viidle.isReady;
}

void _SetUserId(ViidleAdIOSPtr iOSPtr, const char* userId)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    viidleAd.viidle.userId = ViidleUnityCreateNSString(userId);
}

void _ReleaseViidleAd(ViidleAdIOSPtr iOSPtr)
{
    ViidleAd *viidleAd = (__bridge ViidleAd *)iOSPtr;
    ViidleUnityRemoveObject(viidleAd);
}
