//
//  ViidleError.h
//  ViidleSDK
//
//  Copyright (c) 2016年 F@N Communications, Inc. All rights reserved.
//

#import <Foundation/Foundation.h>

extern NSInteger const kViidleRequestStatusCodeSuccess; // 広告要求が問題なくなされた

extern NSInteger const kViidleErrorCodeRequiredParameterEmpty; // 必須パラメータが空もしくは存在しない
extern NSInteger const kViidleErrorCodeContryCodeIsInvalid; // パラメータで渡された国コードが正しくない
extern NSInteger const kViidleErrorCodeAdunitDataIsNotFound; // パラメータの広告枠IDに該当する枠情報が存在しない
extern NSInteger const kViidleErrorCodeAdnetworkIsNotConnected; // 広告枠に接続先となるadnetworkが設定されていない
extern NSInteger const kViidleErrorCodeAppIdIsInvalid; // パラメータで渡されたアプリ識別子が枠情報に設定されているアプリ識別子と一致していない
extern NSInteger const kViidleErrorCodeAdunitIsStopped; // 配信停止中の広告枠だった
extern NSInteger const kViidleErrorCodeAdnetworkEmpty; // AdNetworkが選択されなかった
extern NSInteger const kViidleErrorCodeMediationServiceThrowsException; // メディエーション処理中に何らかのException発生

extern NSInteger const kViidleErrorCodeNetWorkError; // ネットワークエラー
extern NSInteger const kViidleErrorCodeNoFillError; // 動画広告の取得に失敗しました

@interface ViidleError : NSError

@end
