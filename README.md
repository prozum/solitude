[![Travis: Build Status](https://travis-ci.org/prozum/solitude.svg?branch=master)](https://travis-ci.org/prozum/solitude)
[![Build status](https://ci.appveyor.com/api/projects/status/g83t473mgkte1ojg?svg=true)](https://ci.appveyor.com/project/thepalmelund/solitude)
# solitude

An application to help people


Commit Style
--------------------
sln: force unix line endings
readme: add coding style example
samples/sql: added a sql sample
tests/gui.test: fix test
lib/gui: add new widget

Coding Style
--------------------
Indentation: tab

### Example
```C#

namespace Example
{
    public class ExampleClass
    {
        private ExampleProperty
        {
            set;
            get;
        }

        public ExampleClass(int argOne)
        {
            var localVar = argOne;
        }
    }
}
```
