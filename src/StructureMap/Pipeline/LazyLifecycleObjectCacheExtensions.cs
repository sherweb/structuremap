// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Modified by Sherweb in 2019.

using System;
using System.Collections.Concurrent;

namespace StructureMap.Pipeline
{
    public static class LazyLifecycleObjectCacheExtensions
    {
        public static TValue AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, LazyLifecycleObject<TValue>> cache, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            var lazy = cache.AddOrUpdate(key, k => new LazyLifecycleObject<TValue>(() => addValue), (k, currentValue) => new LazyLifecycleObject<TValue>(() => updateValueFactory(k, currentValue.Value)));
            return lazy.Value;
        }

        public static TValue GetOrAdd<TKey, TValue>(this ConcurrentDictionary<TKey, LazyLifecycleObject<TValue>> cache, TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazy = cache.GetOrAdd(key, k => new LazyLifecycleObject<TValue>(() => valueFactory(k)));
            return lazy.Value;
        }

        public static bool TryGetValue<TKey, TValue>(this ConcurrentDictionary<TKey, LazyLifecycleObject<TValue>> cache, TKey key, out TValue value)
        {
            LazyLifecycleObject<TValue> lazy;
            var result = cache.TryGetValue(key, out lazy);

            value = result ? lazy.Value : default(TValue);

            return result;
        }

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, LazyLifecycleObject<TValue>> cache, TKey key, out TValue value)
        {
            LazyLifecycleObject<TValue> lazy;
            var result = cache.TryRemove(key, out lazy);

            value = result && lazy.IsValueCreated ? lazy.Value : default(TValue);

            return result;
        }
    }
}