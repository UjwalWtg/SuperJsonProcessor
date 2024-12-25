#  SuperJsonProcessor

This package provides a set of methods designed for efficient JSON manipulation, validation, and processing. The following core functionalities are included:

- `FlattenJson`: Converts a nested JSON structure into a flat, key-value pair format.
- `UnFlattenJson`: Reverts a flattened JSON structure back to its original nested format.
- `ValidateJson`: Validates a JSON document against a predefined JSON schema.
- `ProcessLargeJson`: Efficiently processes large JSON files in a memory-efficient way.

## Features

### FlattenJson

This method is useful for flattening deeply nested JSON objects into a flat key-value pair structure. It’s commonly used when you need to work with JSON data that needs to be queried or processed in a simpler format.

#### Usage:

string json = "{ 'user': { 'name': 'John', 'details': { 'age': 30, 'city': 'New York' } } }";
var flattenedJson = JsonUtility.FlattenJson(json);
Console.WriteLine(flattenedJson);
// Output: { "user.name": "John", "user.details.age": 30, "user.details.city": "New York" }


### UnFlattenJson

This method restores a previously flattened JSON object back to its original nested structure. It's useful when you want to transform a flat JSON structure back to a hierarchical representation.

#### Usage:

string flattenedJson = "{ \"user.name\": \"John\", \"user.details.age\": 30, \"user.details.city\": \"New York\" }";
var unFlattenedJson = JsonUtility.UnFlattenJson(flattenedJson);
Console.WriteLine(unFlattenedJson);
// Output: { "user": { "name": "John", "details": { "age": 30, "city": "New York" } } }


### ValidateJson

Validates a JSON document against a specified JSON schema. This method ensures that the input JSON follows the required structure and data types as defined in the schema.

#### Usage:

string json = @"{ 'name': 'John', 'age': 30 }";
string schema = @"{
  'type': 'object',
  'properties': {
    'name': {'type': 'string'},
    'age': {'type': 'integer'}
  },
  'required': ['name', 'age']
}";
bool isValid = JsonUtility.ValidateJson(json, schema);
Console.WriteLine(isValid);  // Output: True


### ProcessLargeJson

This method allows you to process large JSON files efficiently by reading them token by token. It's particularly useful for handling large JSON files that can't fit into memory all at once.

#### Usage:

string filePath = "path/to/large.json";
Action<JToken> processAction = (token) =>
{
    // Process each token, for example, print its name field
    Console.WriteLine(token["name"]);
};

JsonProcessor.ProcessLargeJson(filePath, processAction);

## Installation
To install the package,use a package manager like

Install-Package SuperJsonProcessor

## Requirements
.NET Core 3.1 or higher
Newtonsoft.Json (for JSON manipulation)

## License
This package is licensed under the MIT License. See the LICENSE file for more information.

## Acknowledgements
Newtonsoft.Json for JSON parsing and manipulation.
Newtonsoft.Json.Schema for JSON schema validation.

## Contact
For any questions or issues, feel free to open an issue on GitHub or contact the maintainer at ujwalwatgule@gmail.com
