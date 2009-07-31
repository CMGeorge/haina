

**** 注意  2.14版本 存在 g_unichar_get_script() ，但在2.12并不存在

pango依赖此函数，所以在guniprop.c添加g_unichar_get_script()
但此函数并未实现，所以可能会出现问题，需要进一步测试