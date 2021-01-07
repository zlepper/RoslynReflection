## Security considerations
Because this library also scans all dependent assemblies to be able to properly resolve
type, type initializers (Static constructors) are run, and can thus execute arbitrary
code in your compile process, such injecting new code into the compilation. 