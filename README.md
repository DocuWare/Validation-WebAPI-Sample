# Sample REST service for validation

## A sample REST validation engine provided as WebAPI for use with DocuWare Cloud and DocuWare 6.12 (and above)


Since the release of DocuWare 6.12 it is possible to easily verify all index fields before storing to the file cabinet. It is comfortable to use and flexible for many scenarios.

Create dynamic mandatory fields or lookups to external data or to DocuWare before storing. The user is being directly notified in the DocuWare WebClient in case of wrong entries.

The validation inside DocuWare requires a REST service in order to send all index entries to this. With this sample application we're going to show you how easily you can setup your own validation webservice.

##JSON send by DocuWare to validation service

```c#
{
	"TimeStamp": "2017-04-07T13:10:44.8888824Z",
	"UserName": "John Doe",
	"OrganizationName": "Peters Engineering",
	"FileCabinetGuid": "8d36692d-e694-4d8c-93db-e97c98897515",
	"DialogGuid": "ac07d242-0120-4575-8722-7c5ae7286026",
	"DialogType": "InfoDialog",
	"Values": [{
		"FieldName": "TEXTFIELD",
		"ItemElementName": "String",
		"Item": "some text"
	},
	{
		"FieldName": "INTFIELD",
		"ItemElementName": "Int",
		"Item": 1234
	},
	{
		"FieldName": "DECIMALFIELD",
		"ItemElementName": "Decimal",
		"Item": 123.45
	},
	{
		"FieldName": "MEMOFIELD",
		"ItemElementName": "Memo",
		"Item": "Long long long long long text"
	},
	{
		"FieldName": "DATEFIELD",
		"ItemElementName": "Date",
		"Item": "2017-04-01T00:00:00Z"
	},
	{
		"FieldName": "DATETIMEFIELD",
		"ItemElementName": "DateTime",
		"Item": "2017-04-02T12:30:00Z"
	},
	{
		"FieldName": "KEYWORDFIELD",
		"ItemElementName": "Keywords",
		"Item": {
			"Keyword": ["keyword1", "keyword2","keyword3"]
		}
	}]
}

```

#### TimeStamp
Time stamp of the request. Datetime in UTC formatted in the Roundtrip format

#### UserName
The long name of the DocuWare user that requests the validation.

#### OrganizationName
The name of the organization containing the dialog.

####  FileCabinetGuid
Guid of the file cabinet containing the dialog.

#### DialogGuid
Guid of the dialog for which the validation is made.

#### DialogType
Type of the dialog. Available values are:

> InfoDialog - Info dialog for editing index values.

> Store - Store dialog for storing new documents in the file cabinet.
Values
A list of values to be validated. Each value contains the following elements:

#### FieldName
the db name of the field.
### ItemElementName
 string value representing the type of the field. Can be one of the following:
- String - Value element is string in quotation marks. Example: "Some text"
- Int - Value element is Int32 or Int64 formatted in Invariant culture. Example: 1243
- Decimal - Value element is Decimal formatted in Invariant culture. Example: 123.45
- Memo - Value element element is string in quotation marks. "Some long text"
- Date - Date in UTC formatted in the Roundtrip format always in midnight. Example: "2017-04-01T00:00:00Z"
- DateTime - Datetime in UTC formatted in the Roundtrip format. Example: "2017-04-02T12:30:00Z"
- Keywords - Element contains subelement "Keyword" with value array of strings. Example: {"Keyword": ["keyword1", "keyword2","keyword3"]}
- Item - The value of the field formatted as described above.

## Expected JSON response format by DocuWare
> Make sure you always return HTTP status code 200. Because this is used for testing the availability of the validation service!

The expected response is JSON with the following structure:
```c#
// Successful validation
{
	"Status": "OK",
	"Reason": "Everything is fine"
}

// Failed validation
{
	"Status": "Failed",
	"Reason": "Your input is not OK! Check the values!"
}
```
#### Status
The status of the validation. For successful validation the expected value is "OK". Every other value indicates that the validation has failed.

#### Reason (Optional)
Reason for the failed validation. This is the message that is shown to the user.

## Register validation service in DocuWare
- Open DocuWare configuration and edit the details of the desired store dialog. 
> You can also configure validation in info dialogs. So it will be triggered on every updated document.

- Run the Project, get the Server URL and adapt the following URL: http://localhost:63486/api/validation/post
- After testing the availability of the service you can store your changes.
- Now you're ready to complete validations before storing. Any unhandled error message will be displayed in the command prompt. All handled validation errors will be directly displayed by the web client.

## Supported DocuWare versions
These sample validations requires an installation of DocuWare 6.12 system or higher.

### Features
1. Check for amount on invoices
For every stored document by the doc. type “invoice” an amount must be provided. You can setup your DBFields in the config file accordingly.  

2. Check for already existing invoices before storing
This is a bit more complex check. Before storing any document of doc.type “invoice” it will check via a Platform REST API call if a similar document with the same document number, document date and supplier is already existing. 
> _ NOTE: Configure DocuWare connection and search dialog GUID accordingly in the settings file. _ 

3. Check for correct due date
By this we make sure that the due date is in future. 

4. Lookup for project ID into external system
The demo application also holds a check for correct project IDs. However this one is only a sample check with no real database connection. It will only check if the project ID is >= 100. Otherwise the actual store will fail. 


