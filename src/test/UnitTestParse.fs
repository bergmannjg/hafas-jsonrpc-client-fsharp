// export VSTEST_HOST_DEBUG=1
module test

open NUnit.Framework
open Response
open FSharp.Data

let locationResponse = """
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
]"""

let journeyResponse = """
{
    "earlierRef": "2|OB|MT#11#357938#357920#358090#358090#0#0#485#357882#1#-2147483640#0#1#2|PDH#b4be79f6b40f68820646ba26252cd05d|RD#17082020|RT#124230|US#1",
    "laterRef": "2|OF|MT#11#357938#357938#358090#358090#0#0#485#357882#2#-2147482606#0#1#2|PDH#b4be79f6b40f68820646ba26252cd05d|RD#17082020|RT#124230|US#1",
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
                            "station": {
                                "type": "station",
                                "id": "8011160",
                                "name": "Berlin Hbf (tief)",
                                "location": {
                                    "type": "location",
                                    "id": "8011160",
                                    "latitude": 52.524924,
                                    "longitude": 13.369629
                                }
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
                    "arrival": "2020-08-17T15:09:00+02:00",
                    "plannedArrival": "2020-08-17T15:05:00+02:00",
                    "arrivalDelay": 240,
                    "departure": "2020-08-17T12:38:00+02:00",
                    "plannedDeparture": "2020-08-17T12:38:00+02:00",
                    "departureDelay": null,
                    "reachable": true,
                    "tripId": "1|220724|0|80|17082020",
                    "line": {
                        "type": "line",
                        "id": "ice-547",
                        "fahrtNr": "547",
                        "name": "ICE 547",
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
                    "loadFactor": "very-high"
                }
            ],
            "refreshToken": "¶HKI¶T$A=1@O=Bielefeld Hbf@L=8000036@a=128@$A=1@O=Berlin Hbf (tief)@L=8098160@a=128@$202008171238$202008171505$ICE  547$$1$$$",
            "price": null
        },
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
                    "arrival": "2020-08-17T14:18:00+02:00",
                    "plannedArrival": "2020-08-17T14:18:00+02:00",
                    "arrivalDelay": 0,
                    "departure": "2020-08-17T13:20:00+02:00",
                    "plannedDeparture": "2020-08-17T13:20:00+02:00",
                    "departureDelay": 0,
                    "reachable": true,
                    "tripId": "1|215486|0|80|17082020",
                    "line": {
                        "type": "line",
                        "id": "ic-2049",
                        "fahrtNr": "2049",
                        "name": "IC 2049",
                        "public": true,
                        "adminCode": "80____",
                        "mode": "train",
                        "product": "national",
                        "operator": {
                            "type": "operator",
                            "id": "db-fernverkehr-ag",
                            "name": "DB Fernverkehr AG"
                        }
                    },
                    "direction": "Dresden Hbf",
                    "arrivalPlatform": "10",
                    "plannedArrivalPlatform": "10",
                    "departurePlatform": "2",
                    "plannedDeparturePlatform": "2",
                    "loadFactor": "low-to-medium"
                },
                {
                    "origin": {
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
                    "arrival": "2020-08-17T16:11:00+02:00",
                    "plannedArrival": "2020-08-17T16:10:00+02:00",
                    "arrivalDelay": 60,
                    "departure": "2020-08-17T14:32:00+02:00",
                    "plannedDeparture": "2020-08-17T14:31:00+02:00",
                    "departureDelay": 60,
                    "reachable": true,
                    "tripId": "1|225363|0|80|17082020",
                    "line": {
                        "type": "line",
                        "id": "ice-849",
                        "fahrtNr": "849",
                        "name": "ICE 849",
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
                    "arrivalPlatform": "5 D - G",
                    "plannedArrivalPlatform": "5 D - G",
                    "departurePlatform": "9",
                    "plannedDeparturePlatform": "9",
                    "loadFactor": "very-high"
                }
            ],
            "refreshToken": "¶HKI¶T$A=1@O=Bielefeld Hbf@L=8000036@a=128@$A=1@O=Hannover Hbf@L=8000152@a=128@$202008171320$202008171418$IC  2049$$1$$$§T$A=1@O=Hannover Hbf@L=8000152@a=128@$A=1@O=Berlin Hbf (tief)@L=8098160@a=128@$202008171431$202008171610$ICE  849$$1$$$",
            "price": {
                "amount": 80.3,
                "currency": "EUR",
                "hint": null
            }
        }
    ]
}
"""

[<SetUp>]
let Setup () = ()

[<Test>]
let TestLocationResponse () =
    let stops =
        parseStops (JsonValue.Parse locationResponse)

    Assert.That(stops.Length, Is.EqualTo(1))

[<Test>]
let TestJourneyResponse () =
    let journeys =
        parseJourneys (JsonValue.Parse journeyResponse)

    match journeys.journeys with
    | Some arr -> Assert.That(arr.Length, Is.EqualTo(2))
    | None -> Assert.Fail()
