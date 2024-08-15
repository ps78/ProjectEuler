using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests;

public abstract class UnitTestBase
{
    protected readonly ITestOutputHelper testOutput;

    public UnitTestBase(ITestOutputHelper testOutputHelper)
    {
        testOutput = testOutputHelper;
    }
}
