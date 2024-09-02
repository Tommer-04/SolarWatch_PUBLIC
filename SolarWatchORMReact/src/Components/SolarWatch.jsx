import React, { useState } from 'react';
import { format } from 'date-fns';


const SolarWatch = () => {
  const [cityName, setCityName] = useState('');
  const [date, setDate] = useState('');

  const [displayedCityName, setDisplayedCityName] = useState('');
  const [displayedDate, setDisplayedDate] = useState('');
  const [sunrise, setSunrise] = useState('');
  const [sunset, setSunset] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const response = await fetch(`/api/SolarWatch/getData?CityName=${cityName}&Date=${date}`);
      if (!response.ok) {
        const errorData = await response.json();
        if (response.status === 404) {
          setError(errorData.message || 'An error occurred');
        } else {
          setError('Failed to fetch data');
        }
        return;
      }
      const data = await response.json();
      setDisplayedCityName(cityName);
      setDisplayedDate(date);
      setSunrise(data.sunrise);
      setSunset(data.sunset);
    } catch (err) {
      setError(err.message);
    }
  };

  const calculateDaylightPercentage = () => {
    if (!sunrise || !sunset) return 0;

    const sunriseTime = new Date(sunrise);
    const sunsetTime = new Date(sunset);
  
    const totalDayMs = 24 * 60 * 60 * 1000;
    let daylightDurationMs;
  
    if (sunsetTime < sunriseTime) {
      daylightDurationMs = (sunsetTime - sunriseTime) + totalDayMs;
    } else {
      daylightDurationMs = sunsetTime - sunriseTime;
    }
  
    return (daylightDurationMs / totalDayMs) * 100;
  };

  const daylightPercentage = calculateDaylightPercentage();

  return (
    <div className="container">
      <h1>Solar Watch</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            City Name:
            <input
              type="text"
              value={cityName}
              onChange={(e) => setCityName(e.target.value)}
              required
            />
          </label>
        </div>
        <div>
          <label>
            Date:
            <input
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
              required
            />
          </label>
        </div>
        <button type="submit">Get Solar Data</button>
      </form>
      {error && <p className="error">{error}</p>}
      {sunrise && sunset && (
        <div className="result">
          <h2>Solar Data for {displayedCityName} on {displayedDate}:</h2>
          <p>Sunrise: {format(sunrise, 'HH:mm:ss')} UTC</p>
          <p>Sunset: {format(sunset, 'HH:mm:ss')} UTC</p>
          <div className="daylight-bar">
            <div
              className="daylight-bar-fill"
              style={{ width: `${daylightPercentage}%` }}
            ></div>
          </div>
          <p>{daylightPercentage.toFixed(2)}% of the day is daylight</p>
        </div>
      )}
    </div>
  );
};

export default SolarWatch;

