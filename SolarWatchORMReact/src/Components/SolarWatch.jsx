import React, { useState } from 'react';

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
        throw new Error('Failed to fetch data');
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

    const daylightDurationMs = sunsetTime - sunriseTime;

    const totalDayMs = 24 * 60 * 60 * 1000;

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
          <h2>Solar Data for {displayedCityName} on {displayedDate}</h2>
          <p>Sunrise: {new Date(sunrise).toLocaleTimeString()}</p>
          <p>Sunset: {new Date(sunset).toLocaleTimeString()}</p>
          <div className="daylight-bar">
            <div
              className="daylight-bar-fill"
              style={{ width: `${daylightPercentage}%` }}
            ></div>
          </div>
          <p>{daylightPercentage.toFixed(2)}% of the day is daylight</p>
        </div>
      )}
      <style jsx>{`
        .container {
          max-width: 600px;
          margin: 0 auto;
          text-align: center;
        }
        .daylight-bar {
          width: 100%;
          background-color: #e0e0e0;
          height: 20px;
          margin-top: 20px;
          border-radius: 10px;
          overflow: hidden;
        }
        .daylight-bar-fill {
          height: 100%;
          background-color: #ffcc00;
          transition: width 0.3s ease-in-out;
        }
      `}</style>
    </div>
  );
};

export default SolarWatch;

