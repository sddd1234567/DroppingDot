//
//  ViidleUnityGlobal.m
//  Unity-iPhone
//

#import "ViidleUnityGlobal.h"

static NSMutableDictionary *objectCache;

NSString* ViidleUnityCreateNSString(const char* string)
{
    if (string) {
        return @(string);
    } else {
        return @"";
    }
}

void ViidleUnityCacheObject(NSObject *object)
{
    if (!objectCache) {
        objectCache = [NSMutableDictionary new];
    }
    objectCache[@(object.hash)] = object;
}

void ViidleUnityRemoveObject(NSObject *object)
{
    if (objectCache) {
        [objectCache removeObjectForKey:@(object.hash)];
    }
}
