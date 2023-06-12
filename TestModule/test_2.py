# coding: utf-8
import time

import asyncio
from asyncio import test_utils

@asyncio.coroutine
def sleeping():
    loop = asyncio.get_event_loop()
    for i in range(3):
        yield from loop.run_in_executor(None, time.sleep, 1)
        print(i)
    return 'print'

loop = asyncio.new_event_loop()
asyncio.set_event_loop(loop)

print('=== 一つだけ実行してみよう ===')
console = loop.run_until_complete(sleeping())
print(console)
input()