//
//  Viidle.h
//  ViidleSDK
//
//  Copyright © 2016年 F@N Communications, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@protocol ViidleDelegate <NSObject>

@optional

- (void)viidleLoadCompleted;
- (void)viidleLoadFailedWithErrorCode:(NSInteger)errorCode errorMessage:(NSString *)errorMessage;
- (void)viidleShowVideoFailed;
- (void)viidleVideoDisplayed;
- (void)viidleVideoCompleted;
- (void)viidleRewardCompletedWithCurrencyName:(NSString *)currencyName currencyAmount:(NSInteger)currencyAmount;

@end

@interface Viidle : NSObject

@property (nonatomic, weak) id<ViidleDelegate> delegate;
@property (nonatomic, copy) NSString *userId;

-(instancetype)init NS_UNAVAILABLE;
-(instancetype)initWithUnitId:(NSString *)unitId NS_DESIGNATED_INITIALIZER;
-(void)setAutoReload:(BOOL) isAutoReload;
-(void)load;
@property (getter=isReady, readonly) BOOL ready;
-(void)showWithViewController:(UIViewController *)viewController;

-(void)presentTestModeWithViewController:(UIViewController *)viewController;

@end

NS_ASSUME_NONNULL_END
