﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZWave.Channel;

namespace ZWave.CommandClasses
{
    public class ThermostatSetpoint : CommandClassBase
    {
        enum command : byte
        {
            Set = 0x01,
            Get = 0x02,
            Report = 0x03
        }

        public event EventHandler<ReportEventArgs<ThermostatSetpointReport>> Changed;

        public ThermostatSetpoint(Node node)
            : base(node, CommandClass.ThermostatSetpoint)
        {
        }

        public async Task<ThermostatSetpointReport> Get(ThermostatSetpointType type)
        {
            var response = await Channel.Send(Node, new Command(Class, command.Get, Convert.ToByte(type)), command.Report);
            return new ThermostatSetpointReport(Node, response);
        }

        public async Task Set()
        {
            await Channel.Send(Node, new Command(Class, command.Set, 1, 66, 8, 52));
        }

        protected internal override void HandleEvent(Command command)
        {
            base.HandleEvent(command);

            var report = new ThermostatSetpointReport(Node, command.Payload);
            OnChanged(new ReportEventArgs<ThermostatSetpointReport>(report));
        }

        protected virtual void OnChanged(ReportEventArgs<ThermostatSetpointReport> e)
        {
            var handler = Changed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}