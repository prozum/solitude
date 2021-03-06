[![Travis: Build Status](https://travis-ci.org/prozum/solitude.svg?branch=master)](https://travis-ci.org/prozum/solitude)
[![Build status](https://ci.appveyor.com/api/projects/status/g83t473mgkte1ojg?svg=true)](https://ci.appveyor.com/project/thepalmelund/solitude)
# solitude
Social event administration system

Commit Style
--------------------
- sln: force unix line endings
- readme: add coding style example
- samples/sql: added a sql sample
- tests/gui.test: fix test
- lib/gui: add new widget

Coding Style
--------------------
### Indentation
- tab

### Case
- camelCase: private,local
- PascalCase: public,protected,internal

### Arguments
For method definitions longer than 80 characters the arguments should be divided to one argument per line.

### Example
```C#

namespace Example
{
    public class ExampleClass
    {
        public int ExamplePublic { set; get; }
        private int examplePrivate { set; get; }

        public ExampleClass(int argOne,
                            int argTwo,
                            int argThree,
                            int argFour,
                            int argFive)
        {
            var localVar = argOne;
        }
    }
}
```
