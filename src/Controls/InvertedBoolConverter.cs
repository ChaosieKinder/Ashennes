using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashennes.Controls
{
    /// <summary>
    /// Converts true to false and false to true. Simple as that!
    /// </summary>
    public class InvertedBoolConverter : BaseConverter<bool, bool>
    {
        /// <inheritdoc/>
        public override bool DefaultConvertReturnValue { get; set; } = false;

        /// <inheritdoc />
        public override bool DefaultConvertBackReturnValue { get; set; } = false;

        /// <summary>
        /// Converts a <see cref="bool"/> to its inverse value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
        /// <returns>An inverted <see cref="bool"/> from the one coming in.</returns>
        public override bool ConvertFrom(bool value, CultureInfo? culture = null) => !value;

        /// <summary>
        /// Converts a <see cref="bool"/> to its inverse value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
        /// <returns>An inverted <see cref="bool"/> from the one coming in.</returns>
        public override bool ConvertBackTo(bool value, CultureInfo? culture = null) => !value;
    }


    /// <summary>
    /// Abstract class used to implement converters that implements the ConvertBack logic.
    /// </summary>
    /// <typeparam name="TFrom">Type of the input value.</typeparam>
    /// <typeparam name="TTo">Type of the output value.</typeparam>
    public abstract class BaseConverter<TFrom, TTo> : ValueConverterExtension, ICommunityToolkitValueConverter
    {
        /// <summary>
        /// Default value to return when <see cref="IValueConverter.Convert(object?, Type, object?, CultureInfo?)"/> throws an <see cref="Exception"/>.
        /// This value is used when <see cref="Maui.Options.ShouldSuppressExceptionsInConverters"/> is set to <see langword="true"/>.
        /// </summary>
        public abstract TTo DefaultConvertReturnValue { get; set; }

        /// <summary>
        /// Default value to return when <see cref="IValueConverter.ConvertBack(object?, Type, object?, CultureInfo?)"/> throws an <see cref="Exception"/>.
        /// This value is used when <see cref="Maui.Options.ShouldSuppressExceptionsInConverters"/> is set to <see langword="true"/>.
        /// </summary>
        public abstract TFrom DefaultConvertBackReturnValue { get; set; }

        /// <inheritdoc/>
        public Type FromType { get; } = typeof(TFrom);

        /// <inheritdoc/>
        public Type ToType { get; } = typeof(TTo);

        object? ICommunityToolkitValueConverter.DefaultConvertReturnValue => DefaultConvertReturnValue;
        object? ICommunityToolkitValueConverter.DefaultConvertBackReturnValue => DefaultConvertBackReturnValue;

        /// <summary>
        /// Method that will be called by <see cref="IValueConverter.Convert(object?, Type, object?, CultureInfo?)"/>.
        /// </summary>
        /// <param name="value">The object to convert <typeparamref name="TFrom"/> to <typeparamref name="TTo"/>.</param>
        /// <param name="culture">Culture Info</param>
        /// <returns>An object of type <typeparamref name="TTo"/>.</returns>
        public abstract TTo ConvertFrom(TFrom value, CultureInfo? culture);

        /// <summary>
        /// Method that will be called by <see cref="IValueConverter.ConvertBack(object?, Type, object?, CultureInfo?)"/>.
        /// </summary>
        /// <param name="value">Value to be converted from <typeparamref name="TTo"/> to <typeparamref name="TFrom"/>.</param>
        /// <param name="culture">Culture Info</param>
        /// <returns>An object of type <typeparamref name="TFrom"/>.</returns>
        public abstract TFrom ConvertBackTo(TTo value, CultureInfo? culture);

        /// <inheritdoc/>
        object? ICommunityToolkitValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                ValidateTargetType<TFrom>(targetType);

                var converterValue = ConvertValue<TTo>(value);

                return ConvertBackTo(converterValue, culture);
            }
            catch (Exception ex) when (false) // Options.ShouldSuppressExceptionsInConverters
            {
                Debug.WriteLine(ex);
                return DefaultConvertBackReturnValue;
            }
        }

        /// <inheritdoc/>
        object? ICommunityToolkitValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                ValidateTargetType<TTo>(targetType);

                var converterValue = ConvertValue<TFrom>(value);

                return ConvertFrom(converterValue, culture);
            }
            catch (Exception ex) when (false) // Options.ShouldSuppressExceptionsInConverters
            {
                Debug.WriteLine(ex);
                return DefaultConvertReturnValue;
            }
        }
    }

    /// <inheritdoc />
    public abstract class ValueConverterExtension : IMarkupExtension<ICommunityToolkitValueConverter>
    {
        /// <inheritdoc />
        public ICommunityToolkitValueConverter ProvideValue(IServiceProvider serviceProvider)
            => (ICommunityToolkitValueConverter)this;

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
            => ((IMarkupExtension<ICommunityToolkitValueConverter>)this).ProvideValue(serviceProvider);

        private protected static bool IsNullable<T>()
        {
            var type = typeof(T);

            if (!type.IsValueType)
            {
                return true; // ref-type
            }

            if (Nullable.GetUnderlyingType(type) is not null)
            {
                return true; // Nullable<T>
            }

            return false; // value-type
        }

        private protected static bool IsValidTargetType<T>(in Type targetType)
        {
            if (IsConvertingToString(targetType) && CanBeConvertedToString())
            {
                return true;
            }

            try
            {
                var instanceOfT = default(T);
                instanceOfT ??= (T?)Activator.CreateInstance(targetType);

                var result = Convert.ChangeType(instanceOfT, targetType);

                return result is not null;
            }
            catch
            {
                return false;
            }

            static bool IsConvertingToString(in Type targetType) => targetType == typeof(string);
            static bool CanBeConvertedToString() => typeof(T).GetMethods().Any(x => x.Name is nameof(ToString) && x.ReturnType == typeof(string));
        }

        private protected static void ValidateTargetType<TTarget>(Type targetType)
        {
            ArgumentNullException.ThrowIfNull(targetType);

            // Ensure TTo can be assigned to the given Target Type
            if (!typeof(TTarget).IsAssignableFrom(targetType) && !IsValidTargetType<TTarget>(targetType))
            {
                throw new ArgumentException($"targetType needs to be assignable from {typeof(TTarget)}.", nameof(targetType));
            }
        }

#pragma warning disable CS8603 // Possible null reference return. If TParam is null (e.g. `string?`), a null return value is expected
        private protected static TParam ConvertParameter<TParam>(object? parameter) => parameter switch
        {
            null when IsNullable<TParam>() => default,
            null when !IsNullable<TParam>() => throw new ArgumentNullException(nameof(parameter), $"Value cannot be null because {nameof(TParam)} is not nullable."),
            TParam convertedParameter => convertedParameter,
            _ => throw new ArgumentException($"Parameter needs to be of type {typeof(TParam)}", nameof(parameter))
        };
#pragma warning restore CS8603 // Possible null reference return.

#pragma warning disable CS8603 // Possible null reference return. If TValue is null (e.g. `string?`), a null return value is expected
        private protected static TValue ConvertValue<TValue>(object? value) => value switch
        {
            null when IsNullable<TValue>() => default,
            null when !IsNullable<TValue>() => throw new ArgumentNullException(nameof(value), $"Value cannot be null because {nameof(TValue)} is not nullable"),
            TValue convertedValue => convertedValue,
            _ => throw new ArgumentException($"Value needs to be of type {typeof(TValue)}", nameof(value))
        };
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <inheritdoc />
    public interface ICommunityToolkitValueConverter : IValueConverter
    {
        /// <summary>
        /// Default value to return when <see cref="Convert(object?, Type, object?, CultureInfo?)"/> throws an <see cref="Exception"/>.
        /// This value is used when <see cref="Maui.Options.ShouldSuppressExceptionsInConverters"/> is set to <see langword="true"/>.
        /// </summary>
        object? DefaultConvertReturnValue { get; }

        /// <summary>
        /// Default value to return when <see cref="ConvertBack(object?, Type, object?, CultureInfo?)"/> throws an <see cref="Exception"/>.
        /// This value is used when <see cref="Maui.Options.ShouldSuppressExceptionsInConverters"/> is set to <see langword="true"/>.
        /// </summary>
        object? DefaultConvertBackReturnValue { get; }

        /// <summary>
        /// Gets the <see cref="Type" /> this converter expects to <see cref="Convert" /> from or <see cref="ConvertBack" /> to.
        /// </summary>
        Type FromType { get; }

        /// <summary>
        /// Gets the <see cref="Type" /> this converter expects to <see cref="Convert" /> to or <see cref="ConvertBack" /> from.
        /// </summary>
        Type ToType { get; }

        /// <summary>
        /// Converts the incoming values to a different object
        /// </summary>
        /// <param name="value">The object to convert</param>
        /// <param name="targetType">Target Type</param>
        /// <param name="parameter">Optional Parameters</param>
        /// <param name="culture">Culture Info</param>
        /// <returns>The converted object</returns>
        new object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture);

        /// <summary>
        /// Converts the object back to the outgoing values
        /// </summary>
        /// <param name="value">The object to convert back</param>
        /// <param name="targetType">Target Type</param>
        /// <param name="parameter">Optional Parameters</param>
        /// <param name="culture">Culture Info</param>
        /// <returns>The object converted back</returns>
        new object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture);

        /// <inheritdoc />
        object? IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Convert(value, targetType, parameter, culture);

        /// <inheritdoc />
        object? IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            ConvertBack(value, targetType, parameter, culture);
    }
}

