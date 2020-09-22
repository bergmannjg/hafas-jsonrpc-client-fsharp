module BRouter

open System

type LonLat = { longitude: float; latitude: float }
type Params = { map: string; lonlats: string }

type Center =
    { longitude: float
      latitude: float
      distance: float }

let private distance2zoom (d: float) =
    if (d < 15.0) then 11.0
    else if (d < 30.0) then 10.0
    else if (d < 70.0) then 9.0
    else if (d < 200.0) then 8.0
    else if (d < 300.0) then 7.0
    else if (d < 500.0) then 6.0
    else if (d < 1000.0) then 5.0
    else if (d < 2000.0) then 4.0
    else if (d < 4000.0) then 3.0
    else 2.0

let private distance (ll1: LonLat, ll2: LonLat) =
    if ((ll1.latitude = ll2.latitude)
        && (ll1.longitude = ll2.longitude)) then
        0.0
    else
        let radlat1 = Math.PI * ll1.latitude / 180.0
        let radlat2 = Math.PI * ll2.latitude / 180.0
        let theta = ll1.longitude - ll2.longitude
        let radtheta = Math.PI * theta / 180.0

        let mutable dist =
            Math.Sin(radlat1)
            * Math.Sin(radlat2)
            + Math.Cos(radlat1)
              * Math.Cos(radlat2)
              * Math.Cos(radtheta)

        if (dist > 1.0) then dist <- 1.0

        dist <- Math.Acos(dist)
        dist <- dist * 180.0 / Math.PI
        dist <- dist * 60.0 * 1.1515
        dist <- dist * 1.609344 // km
        dist

let private latlonMap (map: (float * float) -> float) (ll1: LonLat) (l: LonLat) =
    { longitude = map (ll1.longitude, l.longitude)
      latitude = map (ll1.latitude, l.latitude) }

let private centerOflocations (locations: LonLat []): Center =
    if (locations.Length <= 1) then
        { longitude = 0.0
          latitude = 0.0
          distance = 0.0 }
    else
        let maxV = 1000000.0

        let (lowerLeft, upperRight) =
            locations
            |> Array.fold (fun (ll1, ll2) l -> ((latlonMap Math.Min ll1 l), (latlonMap Math.Max ll2 l)))
                   ({ longitude = maxV; latitude = maxV },
                    { longitude = -1.0 * maxV
                      latitude = -1.0 * maxV })

        let scale = 1000000.0

        let mid x1 x2 =
            Math.Floor((x1 + ((x2 - x1) / 2.0)) * scale)
            / scale

        let lonMid =
            mid lowerLeft.longitude upperRight.longitude


        let latMid =
            mid lowerLeft.latitude upperRight.latitude

        let dh =
            distance
                ({ longitude = lowerLeft.longitude
                   latitude = lowerLeft.latitude },
                 { longitude = lowerLeft.longitude
                   latitude = upperRight.latitude })

        let dv =
            distance
                ({ longitude = lowerLeft.longitude
                   latitude = lowerLeft.latitude },
                 { longitude = upperRight.longitude
                   latitude = lowerLeft.latitude })

        { longitude = lonMid
          latitude = latMid
          distance = dh + dv }

let private locations2params (locations: LonLat []) =
    if locations.Length > 1 then
        let from = locations.[0]
        let ``to`` = locations.[locations.Length - 1]

        let center = centerOflocations (locations)

        let zoom = distance2zoom (center.distance)

        let mutable s = ""

        let lonlat2string (lonlat: LonLat) =
            lonlat.longitude.ToString()
            + ","
            + lonlat.latitude.ToString()
            + ";"

        let s =
            if (locations.Length > 2) then
                locations
                |> Array.fold (fun s curr -> s + lonlat2string (curr)) ""
            else
                ""

        let lonlat =
            (lonlat2string from) + s + (lonlat2string ``to``)

        Some
            ({ map =
                   zoom.ToString()
                   + "/"
                   + center.latitude.ToString()
                   + "/"
                   + center.longitude.ToString()
               lonlats = lonlat })
    else
        None

let getUri (locations: LonLat []) =
    match (locations2params (locations)) with
    | Some p ->
        // the query string may change, it reflects the brouter api
        sprintf "https://brouter.de/brouter-web/#map=%s/osm-mapnik-german_style&lonlats=%s&profile=rail" p.map p.lonlats
    | None -> "https://brouter.de/brouter-web/#map=6/52.281604/10.950834/osm-mapnik-german_style&profile=rail"
