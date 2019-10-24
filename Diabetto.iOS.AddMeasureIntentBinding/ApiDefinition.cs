using System;
using Foundation;
using Intents;
using ObjCRuntime;

namespace Diabetto
{
    // @interface AddMeasureIntent : INIntent
    [Watch(5, 0), NoTV, NoMac, iOS(12, 0)]
    [BaseType(typeof(INIntent))]
    interface AddMeasureIntent
    {
        // @property (readwrite, copy, nonatomic) NSNumber * _Nullable level;
        [NullAllowed, Export("level", ArgumentSemantic.Copy)]
        NSNumber Level { get; set; }

        // @property (readwrite, copy, nonatomic) NSNumber * _Nullable breadunits;
        [NullAllowed, Export("breadunits", ArgumentSemantic.Copy)]
        NSNumber Breadunits { get; set; }

        // @property (readwrite, copy, nonatomic) NSNumber * _Nullable shortinsulin;
        [NullAllowed, Export("shortinsulin", ArgumentSemantic.Copy)]
        NSNumber Shortinsulin { get; set; }
    }

    // @protocol AddMeasureIntentHandling <NSObject>
    [Watch(5, 0), NoTV, NoMac, iOS(12, 0)]
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface AddMeasureIntentHandling
    {
        // @required -(void)handleAddMeasure:(AddMeasureIntent * _Nonnull)intent completion:(void (^ _Nonnull)(AddMeasureIntentResponse * _Nonnull))completion;
        [Abstract]
        [Export("handleAddMeasure:completion:")]
        void HandleAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion);

        // @required -(void)resolveLevelForAddMeasure:(AddMeasureIntent * _Nonnull)intent withCompletion:(void (^ _Nonnull)(AddMeasureLevelResolutionResult * _Nonnull))completion __attribute__((availability(watchos, introduced=6.0))) __attribute__((availability(ios, introduced=13.0)));
        [Watch(6, 0), iOS(13, 0)]
        [Abstract]
        [Export("resolveLevelForAddMeasure:withCompletion:")]
        void ResolveLevelForAddMeasure(AddMeasureIntent intent, Action<INDoubleResolutionResult> completion);

        // @required -(void)resolveBreadunitsForAddMeasure:(AddMeasureIntent * _Nonnull)intent withCompletion:(void (^ _Nonnull)(AddMeasureBreadunitsResolutionResult * _Nonnull))completion __attribute__((availability(watchos, introduced=6.0))) __attribute__((availability(ios, introduced=13.0)));
        [Watch(6, 0), iOS(13, 0)]
        [Abstract]
        [Export("resolveBreadunitsForAddMeasure:withCompletion:")]
        void ResolveBreadunitsForAddMeasure(AddMeasureIntent intent, Action<INDoubleResolutionResult> completion);

        // @required -(void)resolveShortinsulinForAddMeasure:(AddMeasureIntent * _Nonnull)intent withCompletion:(void (^ _Nonnull)(AddMeasureShortinsulinResolutionResult * _Nonnull))completion __attribute__((availability(watchos, introduced=6.0))) __attribute__((availability(ios, introduced=13.0)));
        [Watch(6, 0), iOS(13, 0)]
        [Abstract]
        [Export("resolveShortinsulinForAddMeasure:withCompletion:")]
        void ResolveShortinsulinForAddMeasure(AddMeasureIntent intent, Action<INIntegerResolutionResult> completion);

        // @optional -(void)confirmAddMeasure:(AddMeasureIntent * _Nonnull)intent completion:(void (^ _Nonnull)(AddMeasureIntentResponse * _Nonnull))completion;
        [Export("confirmAddMeasure:completion:")]
        void ConfirmAddMeasure(AddMeasureIntent intent, Action<AddMeasureIntentResponse> completion);
    }

    // @interface AddMeasureIntentResponse : INIntentResponse
    [Watch(5, 0), NoTV, NoMac, iOS(12, 0)]
    [BaseType(typeof(INIntentResponse))]
    [DisableDefaultCtor]
    interface AddMeasureIntentResponse
    {
        // -(instancetype _Nonnull)initWithCode:(AddMeasureIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
        [Export("initWithCode:userActivity:")]
        [DesignatedInitializer]
        IntPtr Constructor(AddMeasureIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

        // @property (readonly, nonatomic) AddMeasureIntentResponseCode code;
        [Export("code")]
        AddMeasureIntentResponseCode Code { get; }
    }

    // @interface AddMeasureLevelResolutionResult : INDoubleResolutionResult
    [Watch(6, 0), iOS(13, 0)]
    [BaseType(typeof(INDoubleResolutionResult))]
    interface AddMeasureLevelResolutionResult
    {
        // +(instancetype _Nonnull)unsupportedForReason:(AddMeasureLevelUnsupportedReason)reason;
        [Static]
        [Export("unsupportedForReason:")]
        AddMeasureLevelResolutionResult UnsupportedForReason(AddMeasureLevelUnsupportedReason reason);
    }

    // @interface AddMeasureBreadunitsResolutionResult : INDoubleResolutionResult
    [Watch(6, 0), iOS(13, 0)]
    [BaseType(typeof(INDoubleResolutionResult))]
    interface AddMeasureBreadunitsResolutionResult
    {
        // +(instancetype _Nonnull)unsupportedForReason:(AddMeasureBreadunitsUnsupportedReason)reason;
        [Static]
        [Export("unsupportedForReason:")]
        AddMeasureBreadunitsResolutionResult UnsupportedForReason(AddMeasureBreadunitsUnsupportedReason reason);
    }

    // @interface AddMeasureShortinsulinResolutionResult : INIntegerResolutionResult
    [Watch(6, 0), iOS(13, 0)]
    [BaseType(typeof(INIntegerResolutionResult))]
    interface AddMeasureShortinsulinResolutionResult
    {
        // +(instancetype _Nonnull)unsupportedForReason:(AddMeasureShortinsulinUnsupportedReason)reason;
        [Static]
        [Export("unsupportedForReason:")]
        AddMeasureShortinsulinResolutionResult UnsupportedForReason(AddMeasureShortinsulinUnsupportedReason reason);
    }
}
