// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Microsoft.Net.Http.Headers
{
    public class SetCookieHeaderValueTest
    {
        public static TheoryData<SetCookieHeaderValue, string> SetCookieHeaderDataSet
        {
            get
            {
                var dataset = new TheoryData<SetCookieHeaderValue, string>();
                var header1 = new SetCookieHeaderValue("name1", "n1=v1&n2=v2&n3=v3")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                    SameSite = SameSiteEnforcementMode.Strict,
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(1),
                    Path = "path1",
                    Secure = true
                };
                dataset.Add(header1, "name1=n1=v1&n2=v2&n3=v3; expires=Sun, 06 Nov 1994 08:49:37 GMT; max-age=86400; domain=domain1; path=path1; secure; samesite=Strict; httponly");

                var header2 = new SetCookieHeaderValue("name2", "");
                dataset.Add(header2, "name2=");

                var header3 = new SetCookieHeaderValue("name2", "value2");
                dataset.Add(header3, "name2=value2");

                var header4 = new SetCookieHeaderValue("name4", "value4")
                {
                    MaxAge = TimeSpan.FromDays(1),
                };
                dataset.Add(header4, "name4=value4; max-age=86400");

                var header5 = new SetCookieHeaderValue("name5", "value5")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                };
                dataset.Add(header5, "name5=value5; expires=Sun, 06 Nov 1994 08:49:37 GMT; domain=domain1");

                return dataset;
            }
        }

        public static TheoryData<string> InvalidSetCookieHeaderDataSet
        {
            get
            {
                return new TheoryData<string>
                {
                    "expires=Sun, 06 Nov 1994 08:49:37 GMT; max-age=86400; domain=domain1",
                    "name=value; expires=Sun, 06 Nov 1994 08:49:37 ZZZ; max-age=86400; domain=domain1",
                    "name=value; expires=Sun, 06 Nov 1994 08:49:37 GMT; max-age=-86400; domain=domain1",
                };
            }
        }

        public static TheoryData<string> InvalidCookieNames
        {
            get
            {
                return new TheoryData<string>
                {
                    "<acb>",
                    "{acb}",
                    "[acb]",
                    "\"acb\"",
                    "a,b",
                    "a;b",
                    "a\\b",
                };
            }
        }

        public static TheoryData<string> InvalidCookieValues
        {
            get
            {
                return new TheoryData<string>
                {
                    { "\"" },
                    { "a,b" },
                    { "a;b" },
                    { "a\\b" },
                    { "\"abc" },
                    { "a\"bc" },
                    { "abc\"" },
                };
            }
        }

        public static TheoryData<IList<SetCookieHeaderValue>, string[]> ListOfSetCookieHeaderDataSet
        {
            get
            {
                var dataset = new TheoryData<IList<SetCookieHeaderValue>, string[]>();
                var header1 = new SetCookieHeaderValue("name1", "n1=v1&n2=v2&n3=v3")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                    SameSite = SameSiteEnforcementMode.Lax,
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(1),
                    Path = "path1",
                    Secure = true
                };
                var string1 = "name1=n1=v1&n2=v2&n3=v3; expires=Sun, 06 Nov 1994 08:49:37 GMT; max-age=86400; domain=domain1; path=path1; secure; samesite=Lax; httponly";

                var header2 = new SetCookieHeaderValue("name2", "value2");
                var string2 = "name2=value2";

                var header3 = new SetCookieHeaderValue("name3", "value3")
                {
                    MaxAge = TimeSpan.FromDays(1),
                };
                var string3 = "name3=value3; max-age=86400";

                var header4 = new SetCookieHeaderValue("name4", "value4")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                };
                var string4 = "name4=value4; expires=Sun, 06 Nov 1994 08:49:37 GMT; domain=domain1";

                dataset.Add(new[] { header1 }.ToList(), new[] { string1 });
                dataset.Add(new[] { header1, header1 }.ToList(), new[] { string1, string1 });
                dataset.Add(new[] { header1, header1 }.ToList(), new[] { string1, null, "", " ", ",", " , ", string1 });
                dataset.Add(new[] { header2 }.ToList(), new[] { string2 });
                dataset.Add(new[] { header1, header2 }.ToList(), new[] { string1, string2 });
                dataset.Add(new[] { header1, header2 }.ToList(), new[] { string1 + ", " + string2 });
                dataset.Add(new[] { header2, header1 }.ToList(), new[] { string2 + ", " + string1 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string1, string2, string3, string4 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", string1, string2, string3, string4) });

                return dataset;
            }
        }

        public static TheoryData<IList<SetCookieHeaderValue>, string[]> ListWithInvalidSetCookieHeaderDataSet
        {
            get
            {
                var dataset = new TheoryData<IList<SetCookieHeaderValue>, string[]>();
                var header1 = new SetCookieHeaderValue("name1", "n1=v1&n2=v2&n3=v3")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                    SameSite = SameSiteEnforcementMode.Strict,
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromDays(1),
                    Path = "path1",
                    Secure = true
                };
                var string1 = "name1=n1=v1&n2=v2&n3=v3; expires=Sun, 06 Nov 1994 08:49:37 GMT; max-age=86400; domain=domain1; path=path1; secure; samesite=Strict; httponly";

                var header2 = new SetCookieHeaderValue("name2", "value2");
                var string2 = "name2=value2";

                var header3 = new SetCookieHeaderValue("name3", "value3")
                {
                    MaxAge = TimeSpan.FromDays(1),
                };
                var string3 = "name3=value3; max-age=86400";

                var header4 = new SetCookieHeaderValue("name4", "value4")
                {
                    Domain = "domain1",
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                };
                var string4 = "name4=value4; expires=Sun, 06 Nov 1994 08:49:37 GMT; domain=domain1;";

                var invalidString1 = "ipt={\"v\":{\"L\":3},\"pt:{\"d\":3},\"ct\":{},\"_t\":44,\"_v\":\"2\"}";

                var invalidHeader2a = new SetCookieHeaderValue("expires", "Sun");
                var invalidHeader2b = new SetCookieHeaderValue("domain", "domain1");
                var invalidString2 = "ipt={\"v\":{\"L\":3},\"pt\":{d\":3},\"ct\":{},\"_t\":44,\"_v\":\"2\"}; expires=Sun, 06 Nov 1994 08:49:37 GMT; domain=domain1";

                var invalidHeader3 = new SetCookieHeaderValue("domain", "domain1")
                {
                    Expires = new DateTimeOffset(1994, 11, 6, 8, 49, 37, TimeSpan.Zero),
                };
                var invalidString3 = "ipt={\"v\":{\"L\":3},\"pt\":{\"d:3},\"ct\":{},\"_t\":44,\"_v\":\"2\"}; domain=domain1; expires=Sun, 06 Nov 1994 08:49:37 GMT";

                dataset.Add(null, new[] { invalidString1 });
                dataset.Add(new[] { invalidHeader2a, invalidHeader2b }.ToList(), new[] { invalidString2 });
                dataset.Add(new[] { invalidHeader3 }.ToList(), new[] { invalidString3 });
                dataset.Add(new[] { header1 }.ToList(), new[] { string1, invalidString1 });
                dataset.Add(new[] { header1 }.ToList(), new[] { invalidString1, null, "", " ", ",", " , ", string1 });
                dataset.Add(new[] { header1 }.ToList(), new[] { string1 + ", " + invalidString1 });
                dataset.Add(new[] { header1 }.ToList(), new[] { invalidString1 + ", " + string1 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { invalidString1, string1, string2, string3, string4 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string1, invalidString1, string2, string3, string4 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string1, string2, invalidString1, string3, string4 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string1, string2, string3, invalidString1, string4 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string1, string2, string3, string4, invalidString1 });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", invalidString1, string1, string2, string3, string4) });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", string1, invalidString1, string2, string3, string4) });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", string1, string2, invalidString1, string3, string4) });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", string1, string2, string3, invalidString1, string4) });
                dataset.Add(new[] { header1, header2, header3, header4 }.ToList(), new[] { string.Join(",", string1, string2, string3, string4, invalidString1) });

                return dataset;
            }
        }

        [Fact]
        public void SetCookieHeaderValue_CtorThrowsOnNullName()
        {
            Assert.Throws<ArgumentNullException>(() => new SetCookieHeaderValue(null, "value"));
        }

        [Theory]
        [MemberData(nameof(InvalidCookieNames))]
        public void SetCookieHeaderValue_CtorThrowsOnInvalidName(string name)
        {
            Assert.Throws<ArgumentException>(() => new SetCookieHeaderValue(name, "value"));
        }

        [Theory]
        [MemberData(nameof(InvalidCookieValues))]
        public void SetCookieHeaderValue_CtorThrowsOnInvalidValue(string value)
        {
            Assert.Throws<ArgumentException>(() => new SetCookieHeaderValue("name", value));
        }

        [Fact]
        public void SetCookieHeaderValue_Ctor1_InitializesCorrectly()
        {
            var header = new SetCookieHeaderValue("cookie");
            Assert.Equal("cookie", header.Name);
            Assert.Equal(string.Empty, header.Value);
        }

        [Theory]
        [InlineData("name", "")]
        [InlineData("name", "value")]
        [InlineData("name", "\"acb\"")]
        public void SetCookieHeaderValue_Ctor2InitializesCorrectly(string name, string value)
        {
            var header = new SetCookieHeaderValue(name, value);
            Assert.Equal(name, header.Name);
            Assert.Equal(value, header.Value);
        }

        [Fact]
        public void SetCookieHeaderValue_Value()
        {
            var cookie = new SetCookieHeaderValue("name");
            Assert.Equal(string.Empty, cookie.Value);

            cookie.Value = "value1";
            Assert.Equal("value1", cookie.Value);
        }

        [Theory]
        [MemberData(nameof(SetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_ToString(SetCookieHeaderValue input, string expectedValue)
        {
            Assert.Equal(expectedValue, input.ToString());
        }

        [Theory]
        [MemberData(nameof(SetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_AppendToStringBuilder(SetCookieHeaderValue input, string expectedValue)
        {
            var builder = new StringBuilder();

            input.AppendToStringBuilder(builder);

            Assert.Equal(expectedValue, builder.ToString());
        }

        [Theory]
        [MemberData(nameof(SetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_Parse_AcceptsValidValues(SetCookieHeaderValue cookie, string expectedValue)
        {
            var header = SetCookieHeaderValue.Parse(expectedValue);

            Assert.Equal(cookie, header);
            Assert.Equal(expectedValue, header.ToString());
        }

        [Theory]
        [MemberData(nameof(SetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParse_AcceptsValidValues(SetCookieHeaderValue cookie, string expectedValue)
        {
            SetCookieHeaderValue header;
            bool result = SetCookieHeaderValue.TryParse(expectedValue, out header);
            Assert.True(result);

            Assert.Equal(cookie, header);
            Assert.Equal(expectedValue, header.ToString());
        }

        [Theory]
        [MemberData(nameof(InvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_Parse_RejectsInvalidValues(string value)
        {
            Assert.Throws<FormatException>(() => SetCookieHeaderValue.Parse(value));
        }

        [Theory]
        [MemberData(nameof(InvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParse_RejectsInvalidValues(string value)
        {
            SetCookieHeaderValue header;
            bool result = SetCookieHeaderValue.TryParse(value, out header);

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ListOfSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_ParseList_AcceptsValidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            var results = SetCookieHeaderValue.ParseList(input);

            Assert.Equal(cookies, results);
        }

        [Theory]
        [MemberData(nameof(ListOfSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParseList_AcceptsValidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            IList<SetCookieHeaderValue> results;
            bool result = SetCookieHeaderValue.TryParseList(input, out results);
            Assert.True(result);

            Assert.Equal(cookies, results);
        }

        [Theory]
        [MemberData(nameof(ListOfSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_ParseStrictList_AcceptsValidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            var results = SetCookieHeaderValue.ParseStrictList(input);

            Assert.Equal(cookies, results);
        }

        [Theory]
        [MemberData(nameof(ListOfSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParseStrictList_AcceptsValidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            IList<SetCookieHeaderValue> results;
            bool result = SetCookieHeaderValue.TryParseStrictList(input, out results);
            Assert.True(result);

            Assert.Equal(cookies, results);
        }

        [Theory]
        [MemberData(nameof(ListWithInvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_ParseList_ExcludesInvalidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            var results = SetCookieHeaderValue.ParseList(input);
            // ParseList aways returns a list, even if empty. TryParseList may return null (via out).
            Assert.Equal(cookies ?? new List<SetCookieHeaderValue>(), results);
        }

        [Theory]
        [MemberData(nameof(ListWithInvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParseList_ExcludesInvalidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            IList<SetCookieHeaderValue> results;
            bool result = SetCookieHeaderValue.TryParseList(input, out results);
            Assert.Equal(cookies, results);
            Assert.Equal(cookies?.Count > 0, result);
        }

        [Theory]
        [MemberData(nameof(ListWithInvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_ParseStrictList_ThrowsForAnyInvalidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            Assert.Throws<FormatException>(() => SetCookieHeaderValue.ParseStrictList(input));
        }

        [Theory]
        [MemberData(nameof(ListWithInvalidSetCookieHeaderDataSet))]
        public void SetCookieHeaderValue_TryParseStrictList_FailsForAnyInvalidValues(IList<SetCookieHeaderValue> cookies, string[] input)
        {
            IList<SetCookieHeaderValue> results;
            bool result = SetCookieHeaderValue.TryParseStrictList(input, out results);
            Assert.Null(results);
            Assert.False(result);
        }
    }
}
