/*
    Copyright 2014 Björn Fyrvall

    This file is part of BonaJson

    BonaJson is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BonaJson is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BonaJson.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaJson.Internal
{
    public class JObjectValue : JObject
    {

        public override void Add(object element)
        {
            throw new InvalidOperationException();
        }
        public override void Add(JObject element)
        {
            throw new InvalidOperationException();
        }
        public override void Add(string name, object element)
        {
            throw new InvalidOperationException();
        }
        public override void Add(string name, JObject child)
        {
            throw new InvalidOperationException();
        }

        public override bool Remove(JObject element)
        {
            throw new InvalidOperationException();
        }

        public override bool Remove(string name)
        {
            throw new InvalidOperationException();
        }

        public override JObject this[string key]
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override void AddNodeToPrettyPrint(Internal.PrettyPrintData data)
        {
            data.Text += m_value.ToString();
        }
    }
}
