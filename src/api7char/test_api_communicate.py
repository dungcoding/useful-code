from flask import Flask, request, jsonify, render_template
from PIL import Image
import numpy as np
import cv2
from HandTracking import HandDetector
from Classification import Classifier
import traceback
import io
import base64
import json

app = Flask(__name__)

@app.route("/", methods=["GET"])
def _hello_world():
	return "Hello world"

img_size = 400
classifier = Classifier("C:\\Users\\Lenovo\\PycharmProjects\\hand_sign_vn\\streamlit_app\\models\\keras_model_7chars.h5",
                        "C:\\Users\\Lenovo\\PycharmProjects\\hand_sign_vn\\streamlit_app\\models\\labels_7chars.txt")
detector = HandDetector(maxHands=1)
hd = HandDetector(maxHands=1)
hd2 = HandDetector(maxHands=1)
labels = ['den truong', 'di', 'duoc', 'giao tiep', 'hoc', 'muon', 'toi']
white = np.ones((img_size, img_size), np.uint8) * 255
cv2.imwrite("C:\\Users\\Lenovo\\PycharmProjects\\hand_sign_vn\\streamlit_app\\white.jpg", white)


class VideoProcessor:
    def __init__(self):
        self.offset = 26
        self.img_size = 400
        self.current_label = {'label': 'example_label'}

    def get_current_label_json(self):
        """
        Return current labels of the image in JSON format.
        """
        return json.dumps(self.current_label)
    def process_frame(self, frm):
        try:
            frm = cv2.flip(frm, 1)
            hands, frm = hd.findHands(frm, draw=False, flipType=True)

            if hands:
                hand = hands[0]
                x, y, w, h = hand['bbox']
                image = frm[y - self.offset:y + h + self.offset, x - self.offset:x + w + self.offset]
                white = cv2.imread("C:\\Users\\Lenovo\\PycharmProjects\\hand_sign_vn\\streamlit_app\\white.jpg")
                handz, image = hd2.findHands(image, draw=True, flipType=True)
                if handz:

                    hand = handz[0]
                    pts = hand['lmList']

                    os = ((self.img_size - w) // 2) - 15
                    os1 = ((self.img_size - h) // 2) - 15
                    for t in range(0, 4, 1):
                        cv2.line(white, (pts[t][0] + os, pts[t][1] + os1), (pts[t + 1][0] + os, pts[t + 1][1] + os1), (0, 255, 0), 3)
                    for t in range(5, 8, 1):
                        cv2.line(white, (pts[t][0] + os, pts[t][1] + os1), (pts[t + 1][0] + os, pts[t + 1][1] + os1), (0, 255, 0), 3)
                    for t in range(9, 12, 1):
                        cv2.line(white, (pts[t][0] + os, pts[t][1] + os1), (pts[t + 1][0] + os, pts[t + 1][1] + os1), (0, 255, 0), 3)
                    for t in range(13, 16, 1):
                        cv2.line(white, (pts[t][0] + os, pts[t][1] + os1), (pts[t + 1][0] + os, pts[t + 1][1] + os1), (0, 255, 0), 3)
                    for t in range(17, 20, 1):
                        cv2.line(white, (pts[t][0] + os, pts[t][1] + os1), (pts[t + 1][0] + os, pts[t + 1][1] + os1), (0, 255, 0), 3)
                    cv2.line(white, (pts[5][0] + os, pts[5][1] + os1), (pts[9][0] + os, pts[9][1] + os1), (0, 255, 0), 3)
                    cv2.line(white, (pts[9][0] + os, pts[9][1] + os1), (pts[13][0] + os, pts[13][1] + os1), (0, 255, 0), 3)
                    cv2.line(white, (pts[13][0] + os, pts[13][1] + os1), (pts[17][0] + os, pts[17][1] + os1), (0, 255, 0), 3)
                    cv2.line(white, (pts[0][0] + os, pts[0][1] + os1), (pts[5][0] + os, pts[5][1] + os1), (0, 255, 0), 3)
                    cv2.line(white, (pts[0][0] + os, pts[0][1] + os1), (pts[17][0] + os, pts[17][1] + os1), (0, 255, 0), 3)

                    for i in range(21):
                        cv2.circle(white, (pts[i][0] + os, pts[i][1] + os1), 2, (0, 0, 255), 1)

                    prediction, index = classifier.getPrediction(white, draw=False)

                    if prediction[index] > 0.9:
                        self.current_label['label'] = labels[index]
                    else:
                        self.current_label['label'] = "Khong nhan ra"
                    frm = cv2.putText(frm, self.current_label['label'], (x, y - 26), cv2.FONT_HERSHEY_COMPLEX, 1.7, (255, 255, 255), 2)
                else:
                    print("khong phat hien tay")
            return frm
        except Exception as e:
            traceback.print_exc()
            return frm


video_processor = VideoProcessor()


# @app.route("/index", methods=["GET"])
# def index():
#     return render_template("index.html")

@app.route('/predict', methods=['POST'])
def predict():
    try:
        if 'image' not in request.files:
            return jsonify({'error': 'no image'}), 400

        image_file = request.files['image']
        image_bytes = image_file.read()
        np_img = np.frombuffer(image_bytes, np.uint8)
        image = cv2.imdecode(np_img, cv2.IMREAD_COLOR)

        if image is None:
            return jsonify({'error': 'cannot decode image'}), 400

        result = video_processor.process_frame(image)
        ret, buffer = cv2.imencode('.jpg', result)
        if not ret:
            return jsonify({'error': 'failed to encode image'}), 500
        """Returns the recognized frame as an image file """
        # frame_bytes = buffer.tobytes()
        #
        # return frame_bytes, 200, {'Content-Type': 'image/jpeg'}
        """Returns the recognized frame as an json file """
        # frame_base64 = base64.b64encode(buffer).decode('utf-8')
        #
        # return jsonify({'image': frame_base64}), 200
        """Return current labels of the image"""
        return video_processor.get_current_label_json()
    except Exception as e:
        traceback.print_exc()
        return jsonify({'error': str(e)}), 500
# Main
if __name__ == '__main__':
    app.run(debug=True)
    # from waitress import serve
    #
    # serve(app, host='0.0.0.0', port=8000)
