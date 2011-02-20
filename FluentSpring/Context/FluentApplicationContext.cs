﻿using System;
using System.Collections.Generic;
using FluentSpring.Context.Configuration;
using FluentSpring.Context.Configuration.Binders;
using FluentSpring.Context.Parsers;
using FluentSpring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace FluentSpring.Context
{
    /// <summary>
    /// This is the entry class to register type and configure DI
    /// 
    /// This is the class where you would change the object definition loader, or the spring object definition factory.
    /// </summary>
    public static class FluentApplicationContext
    {
        //public static Inline Inline
        //{
        //    get { return Inline.Instance; }
        //}

        public static void Clear()
        {
            FluentStaticConfiguration.Clear();
            ConditionalBindingDefinitionParser.Clear();
        }

        public static void SetSpringDefault(AutoWiringMode wiringMode, DependencyCheckingMode dependencyCheckMode)
        {
            FluentStaticConfiguration.DefaultWiringMode = wiringMode;
            FluentStaticConfiguration.DefaultDependencyCheckingMode = dependencyCheckMode;
        }

        public static ICanConfigureObject<T> Register<T>() where T : class
        {
            ICanConfigureObject<T> canBind = Inline.Object<T>();
            FluentStaticConfiguration.RegisterObjectConfiguration(canBind.GetConfigurationParser());
            return canBind;
        }

        public static ICanConfigureObject<T> Register<T>(string identifierName) where T : class
        {
            ICanConfigureObject<T> canBind = Inline.Object<T>(identifierName);
            FluentStaticConfiguration.RegisterObjectConfiguration(canBind.GetConfigurationParser());
            return canBind;
        }

        public static ICanConfigureObject<T> Register<T>(Func<ICanConfigureObject<T>> objectConfigurer) where T : class
        {
            ICanConfigureObject<T> configuration = objectConfigurer();
            FluentStaticConfiguration.RegisterObjectConfiguration(configuration.GetConfigurationParser());
            return configuration;
        }

        /// <summary>
        /// Binds the interface to a registered object. By default the mapped object will be resolved during the application context load, if you want the resolution to happen
        /// at runtime you need to use the Bind<typeparamref name="T"/>(false) method instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICanBindInterface<T> Bind<T>()
        {
            var parser = new ConditionalBindingDefinitionParser(typeof (T));
            FluentStaticConfiguration.RegisterObjectConfiguration(parser);
            return new ConditionalBinder<T>(parser);
        }

        /// <summary>
        /// Same as Bind<typeparamref name="T"/>() method, however the registered object resolution will be executed at runtime (i.e. upon the object's request from the context).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isStatic">if set to <c>true</c> [is static].</param>
        /// <returns></returns>
        public static ICanBindInterface<T> Bind<T>(bool isStatic)
        {
            var parser = new ConditionalBindingDefinitionParser(typeof (T), isStatic);
            FluentStaticConfiguration.RegisterObjectConfiguration(parser);
            return new ConditionalBinder<T>(parser);
        }
    }
}