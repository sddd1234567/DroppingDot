//
//  ViidleLogger.h
//  ViidleSDK
//
//  Copyright © 2016年 F@N Communications, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, ViidleLogLevel) {
    ViidleLogLevelDebug = 1,
    ViidleLogLevelWarn = 2,
    ViidleLogLevelError = 3,
    ViidleLogLevelNone = INT_MAX,
};

@interface ViidleLogger : NSObject

+ (void)setLogLevel:(ViidleLogLevel)level;

@end

NS_ASSUME_NONNULL_END
