using System;
using Foundation;
using Intents;
using ObjCRuntime;

namespace Diabetto
{
	// @interface AddShortInsulinIntent : INIntent
	[Watch (5,0), NoTV, NoMac, iOS (12,0)]
	[BaseType (typeof(INIntent))]
	interface AddShortInsulinIntent
	{
		// @property (readwrite, copy, nonatomic) NSNumber * _Nullable shortInsulin;
		[NullAllowed, Export ("shortInsulin", ArgumentSemantic.Copy)]
		NSNumber ShortInsulin { get; set; }

		// @property (readwrite, copy, nonatomic) NSNumber * _Nullable breadUnits;
		[NullAllowed, Export ("breadUnits", ArgumentSemantic.Copy)]
		NSNumber BreadUnits { get; set; }
	}

	// @protocol AddShortInsulinIntentHandling <NSObject>
	[Watch (5,0), NoTV, NoMac, iOS (12,0)]
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface AddShortInsulinIntentHandling
	{
		// @required -(void)handleAddShortInsulin:(AddShortInsulinIntent * _Nonnull)intent completion:(void (^ _Nonnull)(AddShortInsulinIntentResponse * _Nonnull))completion;
		[Abstract]
		[Export ("handleAddShortInsulin:completion:")]
		void HandleAddShortInsulin (AddShortInsulinIntent intent, Action<AddShortInsulinIntentResponse> completion);

		// @required -(void)resolveShortInsulinForAddShortInsulin:(AddShortInsulinIntent * _Nonnull)intent withCompletion:(void (^ _Nonnull)(AddShortInsulinShortInsulinResolutionResult * _Nonnull))completion __attribute__((availability(watchos, introduced=6.0))) __attribute__((availability(ios, introduced=13.0)));
		[Watch (6,0), iOS (13,0)]
		[Abstract]
		[Export ("resolveShortInsulinForAddShortInsulin:withCompletion:")]
		void ResolveShortInsulinForAddShortInsulin (AddShortInsulinIntent intent, Action<INIntegerResolutionResult> completion);

		// @required -(void)resolveBreadUnitsForAddShortInsulin:(AddShortInsulinIntent * _Nonnull)intent withCompletion:(void (^ _Nonnull)(AddShortInsulinBreadUnitsResolutionResult * _Nonnull))completion __attribute__((availability(watchos, introduced=6.0))) __attribute__((availability(ios, introduced=13.0)));
		[Watch (6,0), iOS (13,0)]
		[Abstract]
		[Export ("resolveBreadUnitsForAddShortInsulin:withCompletion:")]
		void ResolveBreadUnitsForAddShortInsulin (AddShortInsulinIntent intent, Action<INDoubleResolutionResult> completion);

		// @optional -(void)confirmAddShortInsulin:(AddShortInsulinIntent * _Nonnull)intent completion:(void (^ _Nonnull)(AddShortInsulinIntentResponse * _Nonnull))completion;
		[Export ("confirmAddShortInsulin:completion:")]
		void ConfirmAddShortInsulin (AddShortInsulinIntent intent, Action<AddShortInsulinIntentResponse> completion);
	}

	// @interface AddShortInsulinIntentResponse : INIntentResponse
	[Watch (5,0), NoTV, NoMac, iOS (12,0)]
	[BaseType (typeof(INIntentResponse))]
	[DisableDefaultCtor]
	interface AddShortInsulinIntentResponse
	{
		// -(instancetype _Nonnull)initWithCode:(AddShortInsulinIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
		[Export ("initWithCode:userActivity:")]
		[DesignatedInitializer]
		IntPtr Constructor (AddShortInsulinIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

		// @property (readonly, nonatomic) AddShortInsulinIntentResponseCode code;
		[Export ("code")]
		AddShortInsulinIntentResponseCode Code { get; }
	}

	// @interface AddShortInsulinShortInsulinResolutionResult : INIntegerResolutionResult
	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof(INIntegerResolutionResult))]
	interface AddShortInsulinShortInsulinResolutionResult
	{
		// +(instancetype _Nonnull)unsupportedForReason:(AddShortInsulinShortInsulinUnsupportedReason)reason;
		[Static]
		[Export ("unsupportedForReason:")]
		AddShortInsulinShortInsulinResolutionResult UnsupportedForReason (AddShortInsulinShortInsulinUnsupportedReason reason);
	}

	// @interface AddShortInsulinBreadUnitsResolutionResult : INDoubleResolutionResult
	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof(INDoubleResolutionResult))]
	interface AddShortInsulinBreadUnitsResolutionResult
	{
		// +(instancetype _Nonnull)unsupportedForReason:(AddShortInsulinBreadUnitsUnsupportedReason)reason;
		[Static]
		[Export ("unsupportedForReason:")]
		AddShortInsulinBreadUnitsResolutionResult UnsupportedForReason (AddShortInsulinBreadUnitsUnsupportedReason reason);
	}
}
