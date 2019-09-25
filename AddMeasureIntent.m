//
// AddMeasureIntent.m
//
// This file was automatically generated and should not be edited.
//

#import "AddMeasureIntent.h"

#if __has_include(<Intents/Intents.h>) && (!TARGET_OS_OSX || TARGET_OS_IOSMAC) && !TARGET_OS_TV

@implementation AddMeasureIntent

@dynamic level, breadunits, shortinsulin;

@end

@interface AddMeasureIntentResponse ()

@property (readwrite, NS_NONATOMIC_IOSONLY) AddMeasureIntentResponseCode code;

@end

@implementation AddMeasureIntentResponse

@synthesize code = _code;

- (instancetype)initWithCode:(AddMeasureIntentResponseCode)code userActivity:(nullable NSUserActivity *)userActivity {
    self = [super init];
    if (self) {
        _code = code;
        self.userActivity = userActivity;
    }
    return self;
}

@end

@implementation AddMeasureLevelResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureLevelUnsupportedReason)reason {
    return [super unsupportedWithReason:reason];
}

@end

@implementation AddMeasureBreadunitsResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureBreadunitsUnsupportedReason)reason {
    return [super unsupportedWithReason:reason];
}

@end

@implementation AddMeasureShortinsulinResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureShortinsulinUnsupportedReason)reason {
    return [super unsupportedWithReason:reason];
}

@end

#endif
