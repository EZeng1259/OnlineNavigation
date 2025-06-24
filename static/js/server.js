const express = require('express');
const { Pool } = require('pg');
const cors = require('cors'); // Import the CORS middleware
const app = express();
const port = process.env.PORT || 3000;

const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
  ssl: {
    rejectUnauthorized: false
  }
})

// Use CORS middleware
app.use(cors({
  origin: 'https://obscure-ocean-11903-991e28965084.herokuapp.com', // Replace with your Unity WebGL domain
  methods: ['GET', 'POST', 'OPTIONS'],
  allowedHeaders: ['Content-Type', 'Authorization', 'Access-Control-Allow-Origin']
}));

app.use(express.json());

// Handle preflight requests
app.options('*', cors());

app.post('/save-warmupnavigation', async (req, res) => {
  const { participantID, timestamp, x, y, rotx, roty } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO warmupnavigation (participantID, timestamp, x, y, rotx, roty)
       VALUES ($1, $2, $3, $4, $5, $6)`,
      [participantID, timestamp, x, y, rotx, roty]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-navigation', async (req, res) => {
  const { participantID, trialNum, timestamp, x, y, rotx, roty } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO navigation (participantID, trialNum, timestamp, x, y, rotx, roty)
       VALUES ($1, $2, $3, $4, $5, $6, $7)`,
      [participantID, trialNum, timestamp, x, y, rotx, roty]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-buildingvisitedorder', async (req, res) => {
  const { participantID, trialNum, timestamp, buildingName } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO buildingvisitedorder (participantID, trialNum, timestamp, buildingName)
       VALUES ($1, $2, $3, $4)`,
      [participantID, trialNum, timestamp, buildingName]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-freerecall', async (req, res) => {
  const { participant_id, trial, timestamp, building_name } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO freerecall (participantID, trialNum, timeRecorded, buildingName)
       VALUES ($1, $2, $3, $4)`,
      [participant_id, trial, timestamp, building_name]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-randomizedbuildings', async (req, res) => {
  const { participant_id, order_number, building_name } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO randomizedbuildingorder (participantID, orderNumber, buildingName)
       VALUES ($1, $2, $3)`,
      [participant_id, order_number, building_name]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-mapplacementorder', async (req, res) => {
  const { participant_id, timestamp, building_name } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO mapplacementorder (participantID, timeRecorded, buildingName)
       VALUES ($1, $2, $3)`,
      [participant_id, timestamp, building_name]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.post('/save-mapcoordinates', async (req, res) => {
  const { participant_id, store_name, x_pos, y_pos } = req.body;
  try {
    const client = await pool.connect();
    await client.query(
      `INSERT INTO mapcoordinates (participantID, storeName, xPos, yPos)
       VALUES ($1, $2, $3, $4)`,
      [participant_id, store_name, x_pos, y_pos]
    );
    client.release();
    res.send('Data saved successfully');
  } catch (err) {
    console.error(err);
    res.status(500).send('Error saving data');
  }
});

app.listen(port, () => {
  console.log(`Server running on port ${port}`);
});
