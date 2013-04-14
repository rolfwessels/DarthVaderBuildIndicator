from flask import json


class MyEncoder(json.JSONEncoder):
    def default(self, obj):
        # if not isinstance(obj, BaseResponse):
        #     return super(MyEncoder, self).default(obj)
        # if not isinstance(obj, BaseResponse):
        #     return super(MyEncoder, self).default(obj)
        return obj.__dict__


