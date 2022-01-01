#Ferrum ide maker:myusan copyright 2021-
# coding: utf-8
import time

import asyncio
from asyncio import test_utils

@asyncio.coroutine
def Input():
    global scope
    loop = asyncio.get_event_loop()
    for i in range(60):
        yield from loop.run_in_executor(None, time.sleep, 1)
        if scope.getinput(False) != "":
            break
    return scope.getinput(True)
#END

#IF  YOU WANT TO GET SOMETHING,WRITE THIS CODES.
#loop = asyncio.new_event_loop()
#asyncio.set_event_loop(loop)
#console = loop.run_until_complete(Input())
#AND THEN REPLACE "input()" to "console"

#example
#a = input()
#print(a)

#We Must Convert Like This
#loop = asyncio.new_event_loop()
#asyncio.set_event_loop(loop)
#console = loop.run_until_complete(Input())
#a = console
#print(a)