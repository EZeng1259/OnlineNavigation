const express = require('express');
const path = require('path');
const app = express();
const { Pool } = require('pg');

const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
  ssl: {
      rejectUnauthorized: false
  }
});

// Use JSON middleware to parse JSON requests
app.use(express.json());

// Serve the files on the /public directory
app.use(express.static(path.join(__dirname, 'public')));

// Endpoint for warmupnavigation
app.post('/warmupnavigation', async (req, res) => {
  const { participantID, timestamp, x, y, rotx, roty } = req.body;

  if (!participantID || !timestamp) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO warmupnavigation (participantid, timerecorded, x, y, rotx, roty) VALUES ($1, $2, $3, $4, $5, $6)';
  const queryValues = [participantID, timestamp, x, y, rotx, roty];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to warmupnavigation');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to warmupnavigation');
  }
});

// Endpoint for navigation
app.post('/navigation', async (req, res) => {
  const { participantID, trialNum, timestamp, x, y, rotx, roty } = req.body;

  if (!participantID || !trialNum || !timestamp) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO navigation (participantid, trialnum, timerecorded, x, y, rotx, roty) VALUES ($1, $2, $3, $4, $5, $6, $7)';
  const queryValues = [participantID, trialNum, timestamp, x, y, rotx, roty];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to navigation');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to navigation');
  }
});

// Endpoint for buildingvisitedorder
app.post('/buildingvisitedorder', async (req, res) => {
  const { participantID, trialNum, timestamp, buildingName } = req.body;

  if (!participantID || !trialNum || !timestamp || !buildingName) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO buildingvisitedorder (participantid, trialnum, timerecorded, buildingname) VALUES ($1, $2, $3, $4)';
  const queryValues = [participantID, trialNum, timestamp, buildingName];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to buildingvisitedorder');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to buildingvisitedorder');
  }
});

// Endpoint for freerecall
app.post('/freerecall', async (req, res) => {
  const { participantID, timestamp, buildingName } = req.body;

  if (!participantID || !timestamp || !buildingName) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO freerecall (participantid, timerecorded, buildingname) VALUES ($1, $2, $3)';
  const queryValues = [participantID, timestamp, buildingName];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to freerecall');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to freerecall');
  }
});

// Endpoint for randomizedbuildings
app.post('/randomizedbuildings', async (req, res) => {
  const { participantID, orderNum, buildingName } = req.body;

  if (!participantID || !orderNum || !buildingName) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO randomizedbuildings (participantid, ordernumber, buildingname) VALUES ($1, $2, $3)';
  const queryValues = [participantID, orderNum, buildingName];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to randomizedbuildings');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to randomizedbuildings');
  }
});

// Endpoint for mapplacementorder
app.post('/mapplacementorder', async (req, res) => {
  const { participantID, timestamp, buildingName } = req.body;

  if (!participantID || !timestamp || !buildingName) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO mapplacementorder (participantid, timerecorded, buildingname) VALUES ($1, $2, $3)';
  const queryValues = [participantID, timestamp, buildingName];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to mapplacementorder');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to mapplacementorder');
  }
});

// Endpoint for mapcoordinates
app.post('/mapcoordinates', async (req, res) => {
  const { participantID, buildingName, xpos, ypos } = req.body;

  if (!participantID || !buildingName || xpos == null || ypos == null) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO mapcoordinates (participantid, storename, xpos, ypos) VALUES ($1, $2, $3, $4)';
  const queryValues = [participantID, buildingName, xpos, ypos];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to mapcoordinates');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to mapcoordinates');
  }
});

// Endpoint for recordscore
app.post('/recordscore', async (req, res) => {
  const { participantID, freeRecallScore, navigationScore, totalScore } = req.body;

  if (!participantID) {
    return res.status(400).send('Missing required fields');
  }

  const queryText = 'INSERT INTO recordscore (participantid, freerecallscore, navigationscore, totalscore) VALUES ($1, $2, $3, $4)';
  const queryValues = [participantID, freeRecallScore, navigationScore, totalScore];

  try {
    const client = await pool.connect();
    await client.query(queryText, queryValues);
    client.release();
    res.send('Data saved successfully to recordscore');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data to recordscore');
  }
});

// Start the server
const port = process.env.PORT || 3000;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});

