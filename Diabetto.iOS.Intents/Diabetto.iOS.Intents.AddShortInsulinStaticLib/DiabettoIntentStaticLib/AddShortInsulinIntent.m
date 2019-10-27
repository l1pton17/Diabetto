//
// AddShortInsulinIntent.m
//
// This file was automatically generated and should not be edited.
//

#import "AddShortInsulinIntent.h"

#if __has_include(<Intents/Intents.h>) && (!TARGET_OS_OSX || TARGET_OS_IOSMAC) && !TARGET_OS_TV

@implementation AddShortInsulinIntent

@dynamic shortInsulin, breadUnits;

@end

@interface AddShortInsulinIntentResponse ()

@property (readwrite, NS_NONATOMIC_IOSONLY) AddShortInsulinIntentResponseCode code;

@end

@implementation AddShortInsulinIntentResponse

@synthesize code = _code;

- (instancetype)initWithCode:(AddShortInsulinIntentResponseCode)code userActivity:(nullable NSUserActivity *)userActivity {
    self = [super init];
    if (self) {
        _code = code;
        self.userActivity = userActivity;
    }
    return self;
}

@end

@implementation AddShortInsulinShortInsulinResolutionResult

+ (instancetype)unsupportedForReason:(AddShortInsulinShortInsulinUnsupportedReason)reason {
    return [super unsupportedWithReason:reason];
}

@end

@implementation AddShortInsulinBreadUnitsResolutionResult

+ (instancetype)unsupportedForReason:(AddShortInsulinBreadUnitsUnsupportedReason)reason {
    return [super unsupportedWithReason:reason];
}

@end

#endif
