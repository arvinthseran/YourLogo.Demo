using System;
using OpenQA.Selenium;

namespace YourLogo.Web.Driver.ExceptionHandling
{
    public static class WebelementException
    {
        public static T HandleStaleElementException<T>(Func<T> findwebelement)
        {
            const int retry = 5;
            StaleElementReferenceException thisexception = null;
            for (var retryCounter = 0; retryCounter <= retry; retryCounter++)
            {
                try
                {
                    return findwebelement();
                }
                catch (StaleElementReferenceException exception)
                {
                    thisexception = exception;
                }
            }
            throw thisexception;
        }
    }
}
