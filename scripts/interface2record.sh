#!/bin/bash

# mv ts2fable generated interface types to record types

if [  ! -d "./scripts" ]; then
    echo "please run from project directory"
    exit 1
fi

INPUTFILE="../hafas-client-fable/src/HafasClientTypes.fs"
OUTPUTFILE="Types-Hafas.fs"

cat > ${OUTPUTFILE} << EndOfMessage
module rec Hafas

open System

type Promise<'T> =
    abstract _catch: onrejected:option<obj -> 'T> -> Promise<'T>
    abstract _then: onfulfilled:option<'T -> 'TResult> * onrejected:option<obj -> 'TResult> -> Promise<'TResult>

type U2<'a,'b> =
  | Case1 of 'a
  | Case2 of 'b

type U3<'a,'b,'c> =
  | Case1 of 'a
  | Case2 of 'b
  | Case3 of 'c

EndOfMessage

sed '0,/^module CreateClient/d' < ${INPUTFILE} \
| sed -E -e ':a;N;$!ba;s/t\n\n/t\x7d\n/g' \
| sed -e 's/ =/ =\x7b/' \
| sed -e 's/abstract//' \
| sed -e 's/with get, set//' \
| sed -e 's/ReadonlyArray/array/' \
| sed -e 's/..AllowNullLiteral..//' \
| sed -e 's/Item: product:/Item:/' \
| sed -e 's/Item: id:/Item:/' \
| sed -e 's/Item: day:/Item:/' \
| sed -e 's/Item: day:/Item:/' \
| sed -e 's/\[.*Emit.*\]//' \
| sed -e 's/ProductTypeMode =\x7b/ProductTypeMode =/' \
| sed -E -e ':a;N;$!ba;s/Ids =.\n\s+Item\: string -> string../Ids = Map<string,string>/g' \
| sed -E -e ':a;N;$!ba;s/Products =.\n\s+Item\: string -> bool../Products = Map<string,bool>/g' \
| sed -E -e ':a;N;$!ba;s/Facilities =.\n\s+Item\: string -> U2<string, bool>../Facilities = Map<string,U2<string, bool>>/g' \
| sed -e 's/type .*ProductTypeMode =/type ProductTypeMode =/' >> ${OUTPUTFILE}
