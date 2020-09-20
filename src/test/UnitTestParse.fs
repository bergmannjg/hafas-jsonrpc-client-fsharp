// export VSTEST_HOST_DEBUG=1
module test

open NUnit.Framework
open Response
open FSharp.Data
open System.Text.Json
open System.Text.Json.Serialization
open Hafas
open Request
open HafasLibrary

let jsonLocation = """
{
    "type": "location",
    "id": "8000036",
    "latitude": 52.029421,
    "longitude": 8.532777
}"""

let jsonStop = """
{
    "type": "stop",
    "id": "8000036",
    "name": "Bielefeld Hbf",
    "location": {
        "type": "location",
        "id": "8000036",
        "latitude": 52.029421,
        "longitude": 8.532777
    },
    "products": {
        "nationalExpress": true,
        "national": true,
        "regionalExp": true,
        "regional": true,
        "suburban": false,
        "bus": true,
        "ferry": false,
        "subway": false,
        "tram": true,
        "taxi": false
    }
}"""

let jsonLocationResponse = """
{"jsonrpc":"2.0","id":0,"result":
    [
        {
            "type": "stop",
            "id": "8000036",
            "name": "Bielefeld Hbf",
            "location": {
                "type": "location",
                "id": "8000036",
                "latitude": 52.029421,
                "longitude": 8.532777
            },
            "products": {
                "nationalExpress": true,
                "national": true,
                "regionalExp": true,
                "regional": true,
                "suburban": false,
                "bus": true,
                "ferry": false,
                "subway": false,
                "tram": true,
                "taxi": false
            }
        }
    ]
}"""

let jsonJourneyResponse = """
{
    "jsonrpc": "2.0",
    "id": 0,
    "result": {
        "earlierRef": "2|OB|MT#11#405278#405278#405425#405425#0#0#485#405267#1#-2147482606#0#1#2|PDH#15491b976b1997f8141d8cba23782122|RD#19092020|RT#102738|US#1",
        "laterRef": "2|OF|MT#11#405278#405278#405425#405425#0#0#485#405267#1#-2147482606#0#1#2|PDH#15491b976b1997f8141d8cba23782122|RD#19092020|RT#102738|US#1",
        "journeys": [
            {
                "type": "journey",
                "legs": [
                    {
                        "origin": {
                            "type": "stop",
                            "id": "8000036",
                            "name": "Bielefeld Hbf",
                            "location": {
                                "type": "location",
                                "id": "8000036",
                                "latitude": 52.029421,
                                "longitude": 8.532777
                            },
                            "products": {
                                "nationalExpress": true,
                                "national": true,
                                "regionalExp": true,
                                "regional": true,
                                "suburban": false,
                                "bus": true,
                                "ferry": false,
                                "subway": false,
                                "tram": true,
                                "taxi": false
                            }
                        },
                        "destination": {
                            "type": "stop",
                            "id": "8098160",
                            "name": "Berlin Hbf (tief)",
                            "location": {
                                "type": "location",
                                "id": "8098160",
                                "latitude": 52.52585,
                                "longitude": 13.368892
                            },
                            "products": {
                                "nationalExpress": true,
                                "national": true,
                                "regionalExp": true,
                                "regional": true,
                                "suburban": true,
                                "bus": true,
                                "ferry": false,
                                "subway": false,
                                "tram": true,
                                "taxi": false
                            },
                            "station": {
                                "type": "station",
                                "id": "8011160",
                                "name": "Berlin Hbf",
                                "location": {
                                    "type": "location",
                                    "id": "8011160",
                                    "latitude": 52.524924,
                                    "longitude": 13.369629
                                },
                                "products": {
                                    "nationalExpress": true,
                                    "national": true,
                                    "regionalExp": true,
                                    "regional": true,
                                    "suburban": true,
                                    "bus": true,
                                    "ferry": false,
                                    "subway": false,
                                    "tram": true,
                                    "taxi": false
                                }
                            }
                        },
                        "arrival": "2020-09-19T13:22:00+02:00",
                        "plannedArrival": "2020-09-19T13:05:00+02:00",
                        "arrivalDelay": 1020,
                        "departure": "2020-09-19T11:00:00+02:00",
                        "plannedDeparture": "2020-09-19T10:38:00+02:00",
                        "departureDelay": 1320,
                        "reachable": true,
                        "tripId": "1|191707|0|80|19092020",
                        "line": {
                            "type": "line",
                            "id": "ice-545",
                            "fahrtNr": "545",
                            "name": "ICE 545",
                            "public": true,
                            "adminCode": "80____",
                            "mode": "train",
                            "product": "nationalExpress",
                            "operator": {
                                "type": "operator",
                                "id": "db-fernverkehr-ag",
                                "name": "DB Fernverkehr AG"
                            }
                        },
                        "direction": "Berlin Gesundbrunnen",
                        "arrivalPlatform": "6 D - G",
                        "plannedArrivalPlatform": "6 D - G",
                        "departurePlatform": "2",
                        "plannedDeparturePlatform": "2",
                        "stopovers": [
                            {
                                "stop": {
                                    "type": "stop",
                                    "id": "8000036",
                                    "name": "Bielefeld Hbf",
                                    "location": {
                                        "type": "location",
                                        "id": "8000036",
                                        "latitude": 52.029421,
                                        "longitude": 8.532777
                                    },
                                    "products": {
                                        "nationalExpress": true,
                                        "national": true,
                                        "regionalExp": true,
                                        "regional": true,
                                        "suburban": false,
                                        "bus": true,
                                        "ferry": false,
                                        "subway": false,
                                        "tram": true,
                                        "taxi": false
                                    }
                                },
                                "arrival": null,
                                "plannedArrival": null,
                                "arrivalDelay": null,
                                "arrivalPlatform": null,
                                "plannedArrivalPlatform": null,
                                "departure": "2020-09-19T11:00:00+02:00",
                                "plannedDeparture": "2020-09-19T10:38:00+02:00",
                                "departureDelay": 1320,
                                "departurePlatform": "2",
                                "plannedDeparturePlatform": "2",
                                "remarks": [
                                    {
                                        "type": "status",
                                        "code": null,
                                        "text": "points failure"
                                    }
                                ]
                            },
                            {
                                "stop": {
                                    "type": "stop",
                                    "id": "8000152",
                                    "name": "Hannover Hbf",
                                    "location": {
                                        "type": "location",
                                        "id": "8000152",
                                        "latitude": 52.377079,
                                        "longitude": 9.741763
                                    },
                                    "products": {
                                        "nationalExpress": true,
                                        "national": true,
                                        "regionalExp": true,
                                        "regional": true,
                                        "suburban": true,
                                        "bus": true,
                                        "ferry": false,
                                        "subway": false,
                                        "tram": true,
                                        "taxi": false
                                    }
                                },
                                "arrival": "2020-09-19T11:48:00+02:00",
                                "plannedArrival": "2020-09-19T11:28:00+02:00",
                                "arrivalDelay": 1200,
                                "arrivalPlatform": "9",
                                "plannedArrivalPlatform": "9",
                                "departure": "2020-09-19T11:51:00+02:00",
                                "plannedDeparture": "2020-09-19T11:31:00+02:00",
                                "departureDelay": 1200,
                                "departurePlatform": "9",
                                "plannedDeparturePlatform": "9"
                            },
                            {
                                "stop": {
                                    "type": "stop",
                                    "id": "8010404",
                                    "name": "Berlin-Spandau",
                                    "location": {
                                        "type": "location",
                                        "id": "8010404",
                                        "latitude": 52.533787,
                                        "longitude": 13.200947
                                    },
                                    "products": {
                                        "nationalExpress": true,
                                        "national": true,
                                        "regionalExp": true,
                                        "regional": true,
                                        "suburban": true,
                                        "bus": true,
                                        "ferry": false,
                                        "subway": true,
                                        "tram": false,
                                        "taxi": false
                                    }
                                },
                                "arrival": "2020-09-19T13:11:00+02:00",
                                "plannedArrival": "2020-09-19T12:54:00+02:00",
                                "arrivalDelay": 1020,
                                "arrivalPlatform": "6 A - D",
                                "plannedArrivalPlatform": "6 A - D",
                                "departure": null,
                                "plannedDeparture": null,
                                "departureDelay": null,
                                "departurePlatform": null,
                                "plannedDeparturePlatform": null
                            },
                            {
                                "stop": {
                                    "type": "stop",
                                    "id": "8098160",
                                    "name": "Berlin Hbf (tief)",
                                    "location": {
                                        "type": "location",
                                        "id": "8098160",
                                        "latitude": 52.52585,
                                        "longitude": 13.368892
                                    },
                                    "products": {
                                        "nationalExpress": true,
                                        "national": true,
                                        "regionalExp": true,
                                        "regional": true,
                                        "suburban": true,
                                        "bus": true,
                                        "ferry": false,
                                        "subway": false,
                                        "tram": true,
                                        "taxi": false
                                    },
                                    "station": {
                                        "type": "station",
                                        "id": "8011160",
                                        "name": "Berlin Hbf",
                                        "location": {
                                            "type": "location",
                                            "id": "8011160",
                                            "latitude": 52.524924,
                                            "longitude": 13.369629
                                        },
                                        "products": {
                                            "nationalExpress": true,
                                            "national": true,
                                            "regionalExp": true,
                                            "regional": true,
                                            "suburban": true,
                                            "bus": true,
                                            "ferry": false,
                                            "subway": false,
                                            "tram": true,
                                            "taxi": false
                                        }
                                    }
                                },
                                "arrival": "2020-09-19T13:22:00+02:00",
                                "plannedArrival": "2020-09-19T13:05:00+02:00",
                                "arrivalDelay": 1020,
                                "arrivalPlatform": "6 D - G",
                                "plannedArrivalPlatform": "6 D - G",
                                "departure": null,
                                "plannedDeparture": null,
                                "departureDelay": null,
                                "departurePlatform": null,
                                "plannedDeparturePlatform": null
                            }
                        ],
                        "remarks": [
                            {
                                "type": "hint",
                                "text": "Komfort Check-in possible (visit bahn.de/kci for more information)",
                                "code": "komfort-checkin",
                                "summary": "Komfort-Checkin available"
                            },
                            {
                                "type": "hint",
                                "text": "Bordrestaurant",
                                "code": "on-board-restaurant",
                                "summary": "Bordrestaurant available"
                            }
                        ],
                        "loadFactor": "very-high"
                    }
                ],
                "refreshToken": "¶HKI¶T$A=1@O=Bielefeld Hbf@L=8000036@a=128@$A=1@O=Berlin Hbf (tief)@L=8098160@a=128@$202009191038$202009191305$ICE  545$$1$$$",
                "price": {
                    "amount": 76.3,
                    "currency": "EUR",
                    "hint": null
                }
            }
        ],
        "realtimeDataFrom": 1600504113
    }
}
"""

[<SetUp>]
let Setup () = ()

[<Test>]
let TestDeserializeLocationResponse () =
    let options = JsonSerializerOptions()
    options.Converters.Add
        (JsonFSharpConverter
            (JsonUnionEncoding.InternalTag
             ||| JsonUnionEncoding.UnwrapRecordCases
             ||| JsonUnionEncoding.UnwrapOption,
             unionTagName = "type",
             unionTagCaseInsensitive = true))

    let response =
        JsonSerializer.Deserialize<Response<array<U3StationStopLocation>>>(jsonLocationResponse, options)

    let stops = response.result.Value

    Assert.That(stops.Length, Is.EqualTo(1))

[<Test>]
let TestDeserializeLocation () =
    let options = JsonSerializerOptions()
    options.Converters.Add(JsonFSharpConverter())

    let location =
        JsonSerializer.Deserialize<Location>(jsonLocation, options)

    Assert.That(location.id.Value, Is.EqualTo("8000036"))

    let stop =
        JsonSerializer.Deserialize<Stop>(jsonStop, options)

    Assert.That(stop.id, Is.EqualTo("8000036"))

[<Test>]
let TestSerializeLocationsOptions () =
    let jsonOptions =
        JsonSerializerOptions(IgnoreNullValues = true)

    jsonOptions.Converters.Add(JsonFSharpConverter())

    let p: LocationParams =
        { name = "Bielefeld"
          options = defaultLocationsOptions }

    let req =
        { jsonrpc = "2.0"
          id = Some 0
          method = "locations"
          ``params`` = p }

    let s =
        JsonSerializer.Serialize<LocationRequest>(req, jsonOptions)

    Assert.That
        (s,
         Is.EqualTo
             ("""{"jsonrpc":"2.0","id":0,"method":"locations","params":{"name":"Bielefeld","options":{"fuzzy":true,"results":1,"language":"de"}}}"""))

// [<Test>]
let TestDeserializeU2XLocationXStop () =
    let options = JsonSerializerOptions()
    options.Converters.Add
        (JsonFSharpConverter
            (JsonUnionEncoding.InternalTag
             ||| JsonUnionEncoding.UnwrapRecordCases
             ||| JsonUnionEncoding.UnwrapOption,
             unionTagName = "type",
             unionTagCaseInsensitive = true))

    let u2Loc =
        JsonSerializer.Deserialize<U3StationStopLocation>(jsonLocation, options)

    match u2Loc with
    | Location location -> Assert.That(location.id, Is.EqualTo(Some("8000036")))
    | _ -> Assert.Fail("xx")

    let u2Stop =
        JsonSerializer.Deserialize<U3StationStopLocation>(jsonStop, options)

    match u2Stop with
    | Stop stop ->
        Assert.That(stop.id, Is.EqualTo("8000036"))
        Assert.That(stop.products.Value.Count, Is.EqualTo(10))
    | _ -> Assert.Fail("xx")

[<Test>]
let TestDeserializeJourneyResponse () =
    let options = JsonSerializerOptions()
    options.Converters.Add(Serializer.UnionConverter<ProductTypeMode>())
    options.Converters.Add
        (JsonFSharpConverter
            (JsonUnionEncoding.InternalTag
             ||| JsonUnionEncoding.UnwrapRecordCases
             ||| JsonUnionEncoding.UnwrapOption,
             unionTagName = "type",
             unionTagCaseInsensitive = true))

    let response =
        JsonSerializer.Deserialize<Response<JourneysResponse>>(jsonJourneyResponse, options)

    match response.result.Value.journeys with
    | Some arr -> Assert.That(arr.Length, Is.EqualTo(1))
    | None -> Assert.Fail()
