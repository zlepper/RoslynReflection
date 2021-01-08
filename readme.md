## Limitations
* Classes cannot be detected as partial when scanning already compiled assemblies.

## Security considerations
Because this library also scans all dependent assemblies to be able to properly resolve
type, type initializers (Static constructors) are run, and can thus execute arbitrary
code in your compile process, such injecting new code into the compilation. 