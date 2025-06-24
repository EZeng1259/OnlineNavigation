
from flask import Flask, request, jsonify
from flask_cors import CORS
import psycopg2
import logging

app = Flask(__name__)
CORS(app, resources={r"/write_data": {"origins": "https://qiongzhang-organization1.github.io"}})
app.config['CORS_HEADERS'] = 'Content-Type'

# Set up logging
logging.basicConfig(level=logging.INFO)

# Connect to your PostgreSQL database
conn = psycopg2.connect(
    dbname="d7nd1arkvisq0d",
    user="uc15iaa4nqls42",
    password="pdf223fab129678a59584a5bef7f522cafa2a8e4dbc3887f00ccba01f54905d33",
    host="c6sfjnr30ch74e.cluster-czrs8kj4isg7.us-east-1.rds.amazonaws.com",
    port="5432"
)
cursor = conn.cursor()

@app.route('/write_data', methods=['OPTIONS'])
def handle_options():
    response = jsonify({"status": "success"})
    response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
    response.headers.add("Access-Control-Allow-Methods", "GET, POST, OPTIONS")
    response.headers.add("Access-Control-Allow-Headers", "Content-Type")
    return response

@app.route('/write_data', methods=['POST'])
def write_data():
    logging.info(f"Request from origin: {request.origin}")
    data = request.get_json()

    table_name = data['table_name']
    participant_id = data['participantID']  # Note: changed to match your JSON data structure
    trial = data.get('trial')
    timestamp = data['timestamp']
    x = data.get('x')
    y = data.get('y')
    rotx = data.get('rotx')
    roty = data.get('roty')
    building_name = data.get('building_name')
    store_name = data.get('store_name')
    ordering = data.get('ordering')
    x_pos = data.get('x_pos')
    y_pos = data.get('y_pos')

    try:
        if table_name == "navigation":
            if None in [trial, x, y, rotx, roty]:
                response = jsonify({"status": "error", "message": "Missing data for Navigation table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO navigation (participant_id, trial, timestamp, x, y, rotx, roty)
            VALUES (%s, %s, %s, %s, %s, %s, %s)
            """
            cursor.execute(query, (participant_id, trial, timestamp, x, y, rotx, roty))
        elif table_name == "buildingvisitedorder":
            if None in [trial, building_name]:
                response = jsonify({"status": "error", "message": "Missing data for BuildingVisitedOrder table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO buildingvisitedorder (participant_id, trial, timestamp, building_name)
            VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query, (participant_id, trial, timestamp, building_name))
        elif table_name == "mapplacementorder":
            if None in [building_name]:
                response = jsonify({"status": "error", "message": "Missing data for MapPlacementOrder table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO mapplacementorder (participant_id, timestamp, building_name)
            VALUES (%s, %s, %s)
            """
            cursor.execute(query, (participant_id, timestamp, building_name))
        elif table_name == "mapcoordinates":
            if None in [store_name, x_pos, y_pos]:
                response = jsonify({"status": "error", "message": "Missing data for MapCoordinates table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO mapcoordinates (participant_id, store_name, x_pos, y_pos)
            VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query, (participant_id, store_name, x_pos, y_pos))
        elif table_name == "randomizedbuildingorder":
            if None in [ordering, building_name]:
                response = jsonify({"status": "error", "message": "Missing data for RandomizedBuildingOrder table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO randomizedbuildingorder (participant_id, ordering, building_name)
            VALUES (%s, %s, %s)
            """
            cursor.execute(query, (participant_id, ordering, building_name))
        elif table_name == "freerecall":
            if None in [trial, building_name]:
                response = jsonify({"status": "error", "message": "Missing data for FreeRecall table"})
                response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
                return response, 400
            query = """
            INSERT INTO freerecall (participant_id, trial, timestamp, building_name)
            VALUES (%s, %s, %s, %s)
            """
            cursor.execute(query, (participant_id, trial, timestamp, building_name))
        else:
            response = jsonify({"status": "error", "message": "Invalid table name"})
            response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
            return response, 400

        conn.commit()
        response = jsonify({"status": "success"})
        response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
        return response

    except Exception as e:
        conn.rollback()
        response = jsonify({"status": "error", "message": str(e)})
        response.headers.add("Access-Control-Allow-Origin", "https://qiongzhang-organization1.github.io")
        return response, 500

if __name__ == '__main__':
    app.run(debug=True)
