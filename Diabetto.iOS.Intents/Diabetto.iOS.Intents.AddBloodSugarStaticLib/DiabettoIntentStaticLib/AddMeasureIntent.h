//
// AddMeasureIntent.h
//
// This file was automatically generated and should not be edited.
//

#if __has_include(<Intents/Intents.h>)

#import <Intents/Intents.h>

NS_ASSUME_NONNULL_BEGIN

API_AVAILABLE(ios(12.0), watchos(5.0)) API_UNAVAILABLE(macos, tvos)
@interface AddMeasureIntent : INIntent

@property (readwrite, copy, nullable, nonatomic) NSNumber *level;
@property (readwrite, copy, nullable, nonatomic) NSNumber *breadunits;
@property (readwrite, copy, nullable, nonatomic) NSNumber *shortinsulin;

@end

@class AddMeasureIntentResponse;
@class AddMeasureLevelResolutionResult;
@class AddMeasureBreadunitsResolutionResult;
@class AddMeasureShortinsulinResolutionResult;

/*!
 @abstract Protocol to declare support for handling a AddMeasureIntent. By implementing this protocol, a class can provide logic for resolving, confirming and handling the intent.
 @discussion The minimum requirement for an implementing class is that it should be able to handle the intent. The confirmation method is optional. The handling method is always called last, after confirming the intent.
 */
API_AVAILABLE(ios(12.0), watchos(5.0)) API_UNAVAILABLE(macos, tvos)
@protocol AddMeasureIntentHandling <NSObject>

@required

/*!
 @abstract Handling method - Execute the task represented by the AddMeasureIntent that's passed in
 @discussion Called to actually execute the intent. The app must return a response for this intent.

 @param  intent The input intent
 @param  completion The response handling block takes a AddMeasureIntentResponse containing the details of the result of having executed the intent

 @see  AddMeasureIntentResponse
 */
- (void)handleAddMeasure:(AddMeasureIntent *)intent completion:(void (^)(AddMeasureIntentResponse *response))completion NS_SWIFT_NAME(handle(intent:completion:));

/*!
@abstract Resolution methods - Determine if this intent is ready for the next step (confirmation)
@discussion Called to make sure the app extension is capable of handling this intent in its current form. This method is for validating if the intent needs any further fleshing out.

@param  intent The input intent
@param  completion The response block contains an INIntentResolutionResult for the parameter being resolved

@see INIntentResolutionResult
*/
- (void)resolveLevelForAddMeasure:(AddMeasureIntent *)intent withCompletion:(void (^)(AddMeasureLevelResolutionResult *resolutionResult))completion NS_SWIFT_NAME(resolveLevel(for:with:)) API_AVAILABLE(ios(13.0), watchos(6.0));

- (void)resolveBreadunitsForAddMeasure:(AddMeasureIntent *)intent withCompletion:(void (^)(AddMeasureBreadunitsResolutionResult *resolutionResult))completion NS_SWIFT_NAME(resolveBreadunits(for:with:)) API_AVAILABLE(ios(13.0), watchos(6.0));

- (void)resolveShortinsulinForAddMeasure:(AddMeasureIntent *)intent withCompletion:(void (^)(AddMeasureShortinsulinResolutionResult *resolutionResult))completion NS_SWIFT_NAME(resolveShortinsulin(for:with:)) API_AVAILABLE(ios(13.0), watchos(6.0));

@optional

/*!
 @abstract Confirmation method - Validate that this intent is ready for the next step (i.e. handling)
 @discussion Called prior to asking the app to handle the intent. The app should return a response object that contains additional information about the intent, which may be relevant for the system to show the user prior to handling. If unimplemented, the system will assume the intent is valid, and will assume there is no additional information relevant to this intent.

 @param  intent The input intent
 @param  completion The response block contains a AddMeasureIntentResponse containing additional details about the intent that may be relevant for the system to show the user prior to handling.

 @see AddMeasureIntentResponse
 */
- (void)confirmAddMeasure:(AddMeasureIntent *)intent completion:(void (^)(AddMeasureIntentResponse *response))completion NS_SWIFT_NAME(confirm(intent:completion:));

@end

/*!
 @abstract Constants indicating the state of the response.
 */
typedef NS_ENUM(NSInteger, AddMeasureIntentResponseCode) {
    AddMeasureIntentResponseCodeUnspecified = 0,
    AddMeasureIntentResponseCodeReady,
    AddMeasureIntentResponseCodeContinueInApp,
    AddMeasureIntentResponseCodeInProgress,
    AddMeasureIntentResponseCodeSuccess,
    AddMeasureIntentResponseCodeFailure,
    AddMeasureIntentResponseCodeFailureRequiringAppLaunch
} API_AVAILABLE(ios(12.0), watchos(5.0)) API_UNAVAILABLE(macos, tvos);

API_AVAILABLE(ios(12.0), watchos(5.0)) API_UNAVAILABLE(macos, tvos)
@interface AddMeasureIntentResponse : INIntentResponse

- (instancetype)init NS_UNAVAILABLE;

/*!
 @abstract Initializes the response object with the specified code and user activity object.
 @discussion The app extension has the option of capturing its private state as an NSUserActivity and returning it as the 'currentActivity'. If the app is launched, an NSUserActivity will be passed in with the private state. The NSUserActivity may also be used to query the app's UI extension (if provided) for a view controller representing the current intent handling state. In the case of app launch, the NSUserActivity will have its activityType set to the name of the intent. This intent object will also be available in the NSUserActivity.interaction property.

 @param  code The response code indicating your success or failure in confirming or handling the intent.
 @param  userActivity The user activity object to use when launching your app. Provide an object if you want to add information that is specific to your app. If you specify nil, the system automatically creates a user activity object for you, sets its type to the class name of the intent being handled, and fills it with an INInteraction object containing the intent and your response.
 */
- (instancetype)initWithCode:(AddMeasureIntentResponseCode)code userActivity:(nullable NSUserActivity *)userActivity NS_DESIGNATED_INITIALIZER;

/*!
 @abstract The response code indicating your success or failure in confirming or handling the intent.
 */
@property (readonly, NS_NONATOMIC_IOSONLY) AddMeasureIntentResponseCode code;

@end

typedef NS_ENUM(NSInteger, AddMeasureLevelUnsupportedReason) {
    AddMeasureLevelUnsupportedReasonNegativeNumbersNotSupported = 1,
    AddMeasureLevelUnsupportedReasonGreaterThanMaximumValue,
    AddMeasureLevelUnsupportedReasonLessThanMinimumValue,
} API_AVAILABLE(ios(13.0), watchos(6.0));

API_AVAILABLE(ios(13.0), watchos(6.0))
@interface AddMeasureLevelResolutionResult : INDoubleResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureLevelUnsupportedReason)reason;

@end

typedef NS_ENUM(NSInteger, AddMeasureBreadunitsUnsupportedReason) {
    AddMeasureBreadunitsUnsupportedReasonNegativeNumbersNotSupported = 1,
    AddMeasureBreadunitsUnsupportedReasonGreaterThanMaximumValue,
    AddMeasureBreadunitsUnsupportedReasonLessThanMinimumValue,
} API_AVAILABLE(ios(13.0), watchos(6.0));

API_AVAILABLE(ios(13.0), watchos(6.0))
@interface AddMeasureBreadunitsResolutionResult : INDoubleResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureBreadunitsUnsupportedReason)reason;

@end

typedef NS_ENUM(NSInteger, AddMeasureShortinsulinUnsupportedReason) {
    AddMeasureShortinsulinUnsupportedReasonNegativeNumbersNotSupported = 1,
    AddMeasureShortinsulinUnsupportedReasonGreaterThanMaximumValue,
    AddMeasureShortinsulinUnsupportedReasonLessThanMinimumValue,
} API_AVAILABLE(ios(13.0), watchos(6.0));

API_AVAILABLE(ios(13.0), watchos(6.0))
@interface AddMeasureShortinsulinResolutionResult : INIntegerResolutionResult

+ (instancetype)unsupportedForReason:(AddMeasureShortinsulinUnsupportedReason)reason;

@end

NS_ASSUME_NONNULL_END

#endif
