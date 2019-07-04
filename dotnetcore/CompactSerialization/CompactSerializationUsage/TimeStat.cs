// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;

namespace Alachisoft.NCache.Samples
{
	/// <summary>
	/// .
	/// </summary>
	/// <returns></returns>
	[Serializable]
	public class TimeStat
	{
		/// <summary> Total number of samples collected for the statistics. </summary>
		private long		_runCount;
		/// <summary> Total time spent in sampling, i.e., acrued sample time. </summary>
		private long		_totalTime;

		/// <summary> Best time interval mesaured during sampling. </summary>
		private long		_bestTime;
		/// <summary> Worst time interval mesaured during sampling. </summary>
		private long		_worstTime;
		/// <summary> Avg. time interval mesaured during sampling. </summary>
		private float		_avgTime;
		
		/// <summary> Best time interval mesaured during sampling. </summary>
		private long		_expBestTime, _expWorstTime;
		/// <summary> Best time interval mesaured during sampling. </summary>
		private long		_cntBestTime, _cntAvgTime, _cntWorstTime;

		/// <summary> Timestamp for the sampling. </summary>
		private long		_lastStart, _lastStop;


        /// <summary>
        /// Constructor
        /// </summary>
        public TimeStat()
        {
            
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="expBestTime">The expected best time.</param>
		/// <param name="expWorstTime">The expected worst time.</param>
		public TimeStat(long expBestTime, long expWorstTime)
		{
			_expBestTime = expBestTime;
			_expWorstTime = expWorstTime;
			Reset();
		}

		/// <summary> Returns the total numbre of runs in the statistics capture. </summary>
		public long Runs { get { lock(this){ return _runCount; } } }

		/// <summary> Returns the total time iterval spent in sampling </summary>
		public long Total{ get { lock(this){ return _totalTime; } } }

		/// <summary> Returns the time interval for the last sample </summary>
		public long Current{ get { lock(this){ return _lastStop - _lastStart; } } }

		/// <summary> Returns the best/avg/worst time interval mesaured during sampling </summary>
		public long ExpectedBest{ get { lock(this){ return _expBestTime; } } }
		public long ExpectedWorst{ get { lock(this){ return _expWorstTime ; } } }

		/// <summary> Returns the best/avg/worst time interval mesaured during sampling </summary>
		public long Best{ get { lock(this){ return _bestTime; } } }
		public float Avg{ get { lock(this){ return _avgTime ; } } }
		public long Worst { get { lock(this){ return _worstTime; } } }

		/// <summary> Returns the complexity bucket for the current sample </summary>
		public bool IsBestCaseSample{ get{lock(this){ return Current <= _expBestTime; }}}
		public bool IsAvgCaseSample{ get{lock(this){ return (Current > _expBestTime) && (Current < _expWorstTime); }}}
		public bool IsWorstCaseSample{ get{lock(this){ return Current >= _expWorstTime; }}}

		/// <summary> Returns the number of operations per perf. bucket </summary>
		public long BestCases{ get{lock(this){ return _cntBestTime; }}}
		public long AvgCases{ get{lock(this){ return _cntAvgTime; }}}
		public long WorstCases{ get{lock(this){ return _cntWorstTime; }}}

		/// <summary> Returns the percentage of operations per perf. bucket </summary>
		public float PctBestCases{ get{lock(this){ return ((float)BestCases / (float)Runs) * 100; }}}
		public float PctAvgCases{ get{lock(this){ return ((float)AvgCases / (float)Runs) * 100; }}}
		public float PctWorstCases{ get{lock(this){ return ((float)WorstCases / (float)Runs) * 100; }}}

		/// <summary>
		/// Resets the statistics collected so far.
		/// </summary>
		public void Reset()
		{
			_runCount = 0;
			_cntBestTime = _cntAvgTime = _cntWorstTime = 0;
			_totalTime = _bestTime = _worstTime = 0;
			_avgTime = 0;
		}
		
		/// <summary>
		/// Timestamps the start of a sampling interval.
		/// </summary>
		public void BeginSample()
		{
			_lastStart = (DateTime.Now.Ticks - 621355968000000000) / 10000;
		}
			
		/// <summary>
		/// Timestamps the end of interval and calculates the sample time
		/// </summary>
		public void EndSample()
		{
			lock(this)
			{
				_lastStop = (DateTime.Now.Ticks - 621355968000000000) / 10000;
				AddSampleTime(Current);
				if(IsBestCaseSample) ++_cntBestTime;
				else if(IsAvgCaseSample) ++_cntAvgTime;
				else ++_cntWorstTime;
			}
		}
			
		/// <summary>
		/// Adds a specified sample time to the statistics and updates the run count
		/// </summary>
		/// <param name="time">sample time in milliseconds.</param>
		public void AddSampleTime(long time)
		{
			lock(this)
			{
				_runCount ++;
				if(_runCount == 1)
				{
					_avgTime = _totalTime = _bestTime = _worstTime = time;
				}
				else
				{
					_totalTime += time;
					if(time < _bestTime)	_bestTime = time;
					if(time > _worstTime)	_worstTime = time;
					_avgTime = (float)_totalTime / _runCount;
				}
			}
		}

        /// <summary>
        /// This method returns statistics string
        /// </summary>
		public override string ToString()
		{
			lock(this)
			{
				string retval = String.Format("{0,-18}{1,-13}{2,-18}{3,-13}",_runCount, _bestTime, _avgTime, _worstTime);
				return retval;
			}
		}
	}
}

			


