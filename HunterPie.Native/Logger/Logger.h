#pragma once
#if _DEBUG
#define LOG(...) printf_s(__VA_ARGS__)
#else
#define LOG(...)
#endif