package com.alachisoft.ncache.samples.compactSerializationUsage;

public class TimeStat {

    private long        runCount;

    private long        totalTime;

    private long        bestTime;

    private long        worstTime;

    private float        avgTime;

    private long        expBestTime, expWorstTime;

    private long        cntBestTime, cntAvgTime, cntWorstTime;

    private long        lastStart, lastStop;

    public TimeStat(long expBestTime, long expWorstTime)
    {
        expBestTime = expBestTime;
        expWorstTime = expWorstTime;
        reset();
    }

    public long getRunCount() {
        return runCount;
    }

    public void setRunCount(long runCount) {
        this.runCount = runCount;
    }

    public long getTotalTime() {
        return totalTime;
    }

    public void setTotalTime(long totalTime) {
        this.totalTime = totalTime;
    }

    public long getBestTime() {
        return bestTime;
    }

    public void setBestTime(long bestTime) {
        this.bestTime = bestTime;
    }

    public long getWorstTime() {
        return worstTime;
    }

    public void setWorstTime(long worstTime) {
        this.worstTime = worstTime;
    }

    public float getAvgTime() {
        return avgTime;
    }

    public void setAvgTime(float avgTime) {
        this.avgTime = avgTime;
    }

    public long getExpBestTime() {
        return expBestTime;
    }

    public void setExpBestTime(long expBestTime) {
        this.expBestTime = expBestTime;
    }

    public long getExpWorstTime() {
        return expWorstTime;
    }

    public void setExpWorstTime(long expWorstTime) {
        this.expWorstTime = expWorstTime;
    }

    public long getCntBestTime() {
        return cntBestTime;
    }

    public void setCntBestTime(long cntBestTime) {
        this.cntBestTime = cntBestTime;
    }

    public long getCntAvgTime() {
        return cntAvgTime;
    }

    public void setCntAvgTime(long cntAvgTime) {
        this.cntAvgTime = cntAvgTime;
    }

    public long getCntWorstTime() {
        return cntWorstTime;
    }

    public void setCntWorstTime(long cntWorstTime) {
        this.cntWorstTime = cntWorstTime;
    }

    public long getLastStart() {
        return lastStart;
    }

    public void setLastStart(long lastStart) {
        this.lastStart = lastStart;
    }

    public long getLastStop() {
        return lastStop;
    }

    public void setLastStop(long lastStop) {
        this.lastStop = lastStop;
    }

    public TimeStat()
    {

    }

    public long getCurrent()
    {
        synchronized (this)
        {
            return lastStop - lastStart;
        }
    }

    public boolean isBestCaseSample()
    {
        synchronized (this)
        {
            return getCurrent() <= expBestTime;
        }
    }

    public boolean isAvgCaseSample()
    {
        synchronized (this)
        {
            return (getCurrent() > expBestTime) && (getCurrent() < expWorstTime);
        }
    }

    public boolean isWorstCaseSample()
    {
        synchronized (this)
        {
            return getCurrent() >= expWorstTime;
        }
    }

    public long bestCases(){
        synchronized (this){
            return cntBestTime;
        }
    }

    public long avgCases(){
        synchronized (this){
            return cntAvgTime;
        }
    }

    public long worstCases(){
        synchronized (this){
            return cntWorstTime;
        }
    }

    public long runs(){
        synchronized (this){
            return runCount;
        }
    }

    public float pctBestCases(){
        synchronized (this)
        {
            return ((float)bestCases() / (float)runs() * 100);
        }
    }
    public float pctAvgCases(){
        synchronized (this)
        {
            return ((float)avgCases() / (float)runs() * 100);
        }
    }
    public float pctWorstCases(){
        synchronized (this)
        {
            return ((float)worstCases() / (float)runs() * 100);
        }
    }

    public void reset()
    {
        runCount = 0;
        cntBestTime = cntAvgTime = cntWorstTime = 0;
        totalTime = bestTime = worstTime = 0;
        avgTime = 0;
    }

    public void beginSample()
    {
        lastStart = System.currentTimeMillis();
    }

    public void endSample()
    {
        synchronized (this)
        {
            lastStop = System.currentTimeMillis();
            addSampleTime(getCurrent());
            if(isBestCaseSample()) ++cntBestTime;
            else if(isAvgCaseSample())  ++cntAvgTime;
            else ++expWorstTime;
        }
    }

    @Override
    public String toString() {
        synchronized (this) {
            return String.format("%18d %13d %18f %13d", runCount, bestTime, avgTime, worstTime);
        }
    }

    private void addSampleTime(long time) {
        synchronized (this)
        {
            runCount++;
            if(runCount == 1){
                avgTime = totalTime = bestTime = worstTime = time;
            }
            else{
                totalTime += time;
                if(time < bestTime) bestTime = time;
                if(time > worstTime) worstTime = time;
                avgTime = (float)totalTime / runCount;
            }
        }
    }
}
