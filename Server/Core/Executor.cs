using SocketServer.Handlers;
using SocketServer.Ws;
using SocketServer.Attributes;
using SocketServer.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;
using SocketServerw.Core;

namespace SocketServer.Core
{
    class Executor
    {
        private User user;

        private RequestHandler handler;

        private MethodInfo method;

        private Request request;

        public Executor(User user, RequestHandler handler, MethodInfo method, Request request)
        {
            this.user = user;
            this.handler = handler;
            this.method = method;
            this.request = request;
        }

        public void run()
        {
            Response response = new Response();
            response.action = request.Action + Response.ACTION_DONE;
            ServerDiagnostics diag = ServerDiagnostics.GetInstance();
            try
            {
                HandlerArgs args = new HandlerArgs()
                {
                    Caller = this.user,
                    Request = this.request
                };
                handleAttributes(args);

                object result = this.method.Invoke(this.handler, new[] { args });
                response.data = result;
                diag.IncrementTotalHitRequests();
            }
            catch (TargetInvocationException e)
            {
                ExceptionsLogs.Add(e);
                if(e.InnerException is Exception)
                {
                    Debug.WriteLine(e.InnerException);
                    //response.exception = ServerException.createFrom(e.InnerException);
                    response.exception = e.InnerException;
                    diag.IncrementTotalMissRequests();
                }
                else
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                ExceptionsLogs.Add(e);
                response.exception = e;
                diag.IncrementTotalMissRequests();
            }
            finally
            {
                user.send(response);
            }
        }

        private void handleAttributes(HandlerArgs args)
        {
            RequiredChannel requiredChannel = method.GetCustomAttribute<RequiredChannel>();
            if(requiredChannel != null && user.Channel == null && requiredChannel.required.Equals(true))
            {
                throw new Exception("Not in channel");
            }

            if(requiredChannel != null && requiredChannel.required.Equals(false) && user.Channel != null)
            {
                throw new Exception("already in channel");
            }

            RequiredChannelPermissions permissions = method.GetCustomAttribute<RequiredChannelPermissions>();
            if (permissions != null)
            {
                Exception e = new Exception("Invalid permissions");
                if (permissions.ArgNum != null)
                {
                    int id = args.get<int>((int)permissions.ArgNum);
                    if (!DataAccess.channel_permissions.isAllowed(user.Id, id, permissions.level)) throw e;
                }
                else
                {
                    if (!DataAccess.channel_permissions.isAllowed(user.Id, user.Channel.Id, permissions.level)) throw e;
                }
            }
        }
    }
}
