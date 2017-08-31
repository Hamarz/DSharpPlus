﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DSharpPlus.Net
{
    /// <summary>
    /// Represents a request sent over HTTP.
    /// </summary>
    public abstract class BaseRestRequest
    {
        protected internal DiscordClient Discord { get; }
        protected internal TaskCompletionSource<RestResponse> RequestTaskSource { get; }

        /// <summary>
        /// Gets the url to which this request is going to be made.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets the HTTP method used for this request.
        /// </summary>
        public RestRequestMethod Method { get; }

        /// <summary>
        /// Gets the headers sent with this request.
        /// </summary>
        public IReadOnlyDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the rate limit bucket this request is in.
        /// </summary>
        internal RateLimitBucket RateLimitBucket { get; }

        /// <summary>
        /// Creates a new <see cref="BaseRestRequest"/> with specified parameters.
        /// </summary>
        /// <param name="client"><see cref="DiscordClient"/> from which this request originated.</param>
        /// <param name="bucket">Rate limit bucket to place this request in.</param>
        /// <param name="url">Uri to which this request is going to be sent to.</param>
        /// <param name="method">Method to use for this request,</param>
        /// <param name="headers">Additional headers for this request.</param>
        internal BaseRestRequest(DiscordClient client, RateLimitBucket bucket, Uri url, RestRequestMethod method, IDictionary<string, string> headers = null)
        {
            this.Discord = client;
            this.RateLimitBucket = bucket;
            this.RequestTaskSource = new TaskCompletionSource<RestResponse>();
            this.Url = url;
            this.Method = method;
            this.Headers = headers != null ? new ReadOnlyDictionary<string, string>(headers) : null;
        }

        /// <summary>
        /// Asynchronously waits for this request to complete.
        /// </summary>
        /// <returns>HTTP response to this request.</returns>
        public Task<RestResponse> WaitForCompletionAsync() =>
            this.RequestTaskSource.Task;

        protected internal void SetCompleted(RestResponse response) =>
            this.RequestTaskSource.SetResult(response);

        protected internal void SetFaulted(Exception ex) =>
            this.RequestTaskSource.SetException(ex);
    }
}