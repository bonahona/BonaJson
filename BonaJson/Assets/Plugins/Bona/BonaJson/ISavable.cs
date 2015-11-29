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
using BonaJson;

namespace BonaJson
{
    /* This interface set the normal methods classe needs to be able serialize to and from Json in order to save/load to file as well as send over a network connection
     * Author: Björn Fyrvall
     * Deadgnomestudios 2014 */
    public interface ISavable
    {
        JObject Save();
        void Load(JObject source);
    }
}
