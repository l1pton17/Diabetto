//
//  main.m
//  DiabettoIntentCodeGen
//
//  Created by user153408 on 9/25/19.
//  Copyright © 2019 Artur Sitdikov. All rights reserved.
//
#import "AppDelegate.h"
#import "AddShortInsulinIntent.h"

int main(int argc, char * argv[]) {
    NSString * appDelegateClassName;
    @autoreleasepool {
        // Setup code that might create autoreleased objects goes here.
        appDelegateClassName = NSStringFromClass([AppDelegate class]);
    }
    
    return UIApplicationMain(argc, argv, nil, appDelegateClassName);
}
